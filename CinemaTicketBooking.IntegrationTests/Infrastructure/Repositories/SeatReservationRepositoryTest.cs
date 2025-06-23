using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories.Exceptions;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.Repositories;
using CinemaTicketBooking.Infrastructure.DatabaseBindings;
using CinemaTicketBooking.Infrastructure.DatabaseSeeding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.IntegrationTests.Infrastructure.Repositories
{
    [Trait("Category", "LocalDbBasedTests")]
    public class SeatReservationRepositoryTest : TestDatabase
    {
        private readonly IMapper mapper;
        private readonly IDatabaseBinding databaseBinding;

        public SeatReservationRepositoryTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            mapper = mappingConfig.CreateMapper();

            databaseBinding = DatabaseBindingFactory.Create("CinemaTicketBookingDbContext");
        }

        #region AddSeatReservationsAsync Tests

        public static IEnumerable<object[]> AddSeatReservationsAsyncRejectsMultipleReservationsForTheSameSeatAsyncData()
        {
            var seedData = new DefaultSeedData();

            var auditorium = seedData.Auditoriums.First();
            var tier = seedData.Tiers.First(t => t.AuditoriumId == auditorium.Id);
            var movie = seedData.Movies.First();
            var customer = seedData.Customers.First();

            var screening = new Screening
            {
                Id = Guid.NewGuid(),
                AuditoriumId = auditorium.Id,
                MovieId = movie.Id,
                Showtime = DateTimeOffset.Now,
                Language = "English",
                Subtitles = "English"
            };

            var pricing = new Pricing
            {
                Id = Guid.NewGuid(),
                ScreeningId = screening.Id,
                TierId = tier.Id,
                Price = new Price
                {
                    Amount = 4500,
                    Currency = "HUF",
                }
            };

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                BookingState = BookingState.Confirmed,
                CustomerId = customer.Id,
                ScreeningId = screening.Id,
                CreatedOn = DateTimeOffset.UtcNow - TimeSpan.FromDays(3),
            };

            var seatIdToReserve = seedData.Seats.First(s => s.TierId == tier.Id).Id;

            yield return
            [
                seedData,
                screening,
                pricing,
                booking,
                seatIdToReserve
            ];
        }

        [Theory]
        [MemberData(nameof(AddSeatReservationsAsyncRejectsMultipleReservationsForTheSameSeatAsyncData))]
        async Task AddSeatReservationsAsyncRejectsMultipleReservationsForTheSameSeatAsync(SeedData seedData,
                                                                                          Screening screeningToSetup,
                                                                                          Pricing pricingToSetup,
                                                                                          Booking bookingToSetup,
                                                                                          Guid seatIdToReserve)
        {
            // Arrange
            await using var db = await CreateDatabaseAsync(seedData);
            var dbContext = db.Context;
            var seatReservationRepository = new SeatReservationRepository(mapper, databaseBinding, dbContext);
            {
                var screeningRepository = new ScreeningRepository(mapper, dbContext);
                await screeningRepository.AddScreeningsAsync([screeningToSetup]);
                await screeningRepository.SetPricingAsync(pricingToSetup);

                var bookingRepository = new BookingRepository(mapper, dbContext);
                await bookingRepository.AddBookingAsync(bookingToSetup);
            }
            var bookingId = bookingToSetup.Id;
            var screeningId = screeningToSetup.Id;

            var firstSeatReservation = new SeatReservation()
            {
                Id = Guid.NewGuid(),
                BookingId = bookingId,
                ScreeningId = screeningId,
                SeatId = seatIdToReserve,
                Price = new Price { Amount = 10, Currency = "EUR" },
            };
            var secondSeatReservation = new SeatReservation()
            {
                Id = Guid.NewGuid(),
                BookingId = bookingId,
                ScreeningId = screeningId,
                SeatId = seatIdToReserve,
                Price = new Price { Amount = 10, Currency = "EUR" },
            };

            // Act
            var firstCallException = await Record.ExceptionAsync(
                () => seatReservationRepository.AddSeatReservationsAsync([firstSeatReservation]));
            var secondCallException = await Record.ExceptionAsync(
                () => seatReservationRepository.AddSeatReservationsAsync([secondSeatReservation]));

            // Assert
            Assert.Null(firstCallException);
            Assert.IsType<RepositoryException>(secondCallException);
            Assert.IsType<DbUpdateException>(secondCallException.InnerException);
            Assert.IsType<SqlException>(secondCallException.InnerException.InnerException);
            Assert.Null(secondCallException.InnerException.InnerException.InnerException);
            Assert.Equal("Could not reserve seats as at least one of them seems to be already reserved.", secondCallException.Message);
        }

        #endregion

        #region ReleaseSeatsOfTimeoutedBookingsAsync Tests

        public static IEnumerable<object[]> ReleaseSeatsOfTimeoutedBookingsAsyncDeletesTimeoutedSeatReservationsAsyncData()
        {
            var seedData = new DefaultSeedData();

            var auditorium = seedData.Auditoriums.First();
            var tier = seedData.Tiers.First(t => t.AuditoriumId == auditorium.Id);
            var movie = seedData.Movies.First();
            var customer = seedData.Customers.First();

            var sleepyHollowScreening = new Screening
            {
                Id = Guid.NewGuid(),
                AuditoriumId = auditorium.Id,
                MovieId = movie.Id,
                Showtime = DateTimeOffset.Now,
                Language = "English",
                Subtitles = "English"
            };

            var pricing = new Pricing
            {
                Id = Guid.NewGuid(),
                ScreeningId = sleepyHollowScreening.Id,
                TierId = tier.Id,
                Price = new Price
                {
                    Amount = 4500,
                    Currency = "HUF",
                }
            };

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                BookingState = BookingState.ConfirmationTimeout,
                CustomerId = customer.Id,
                ScreeningId = sleepyHollowScreening.Id,
                CreatedOn = DateTimeOffset.UtcNow - TimeSpan.FromDays(3),
            };

            var seatIdToReserve = seedData.Seats.First(s => s.TierId == tier.Id).Id;

            yield return
            [
                seedData,
                sleepyHollowScreening,
                pricing,
                booking,
                seatIdToReserve
            ];
        }

        [Theory]
        [MemberData(nameof(ReleaseSeatsOfTimeoutedBookingsAsyncDeletesTimeoutedSeatReservationsAsyncData))]
        async Task ReleaseSeatsOfTimeoutedBookingsAsyncDeletesTimeoutedSeatReservationsAsync(SeedData seedData,
                                                                                             Screening screeningToSetup,
                                                                                             Pricing pricingToSetup,
                                                                                             Booking bookingToSetup,
                                                                                             Guid seatIdToReserve)
        {
            // Arrange
            await using var db = await CreateDatabaseAsync(seedData);
            var dbContext = db.Context;
            var seatReservationRepository = new SeatReservationRepository(mapper, databaseBinding, dbContext);
            {
                var screeningRepository = new ScreeningRepository(mapper, dbContext);
                await screeningRepository.AddScreeningsAsync([screeningToSetup]);
                await screeningRepository.SetPricingAsync(pricingToSetup);

                var bookingRepository = new BookingRepository(mapper, dbContext);
                await bookingRepository.AddBookingAsync(bookingToSetup);
            }
            var bookingId = bookingToSetup.Id;
            var screeningId = screeningToSetup.Id;

            var seatReservation = new SeatReservation()
            {
                Id = Guid.NewGuid(),
                BookingId = bookingId,
                ScreeningId = screeningId,
                SeatId = seatIdToReserve,
                Price = new Price { Amount = 10, Currency = "EUR" },
            };

            await seatReservationRepository.AddSeatReservationsAsync([seatReservation]);

            // Act
            var seatReservationsBefore = await seatReservationRepository.GetSeatReservationsOfABookingAsync(bookingId);
            await seatReservationRepository.ReleaseSeatsOfTimeoutedBookingsAsync();
            var seatReservationsAfter = await seatReservationRepository.GetSeatReservationsOfABookingAsync(bookingId);

            // Assert
            Assert.NotEmpty(seatReservationsBefore);
            Assert.Empty(seatReservationsAfter);
        }

        #endregion
    }
}
