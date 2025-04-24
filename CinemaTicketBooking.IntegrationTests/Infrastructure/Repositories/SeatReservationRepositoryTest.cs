using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.Repositories;
using CinemaTicketBooking.Infrastructure.DatabaseBindings;
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
            var sopronElitMoziHuszarikTeremDefaultTier = new Tier
            {
                Id = Guid.NewGuid(),
                Name = "default",
                Seats =
                [
                    new Seat
                    {
                        Id = Guid.NewGuid(),
                        Row = 1,
                        Column = 1
                    },
                    new Seat
                    {
                        Id = Guid.NewGuid(),
                        Row = 2,
                        Column = 1
                    },
                    new Seat
                    {
                        Id = Guid.NewGuid(),
                        Row = 3,
                        Column = 1
                    },
                    new Seat
                    {
                        Id = Guid.NewGuid(),
                        Row = 4,
                        Column = 1
                    }
                ]
            };

            var sopronElitMoziHuszarikTerem = new Auditorium
            {
                Id = Guid.NewGuid(),
                Name = "Huszárik Terem",
                Tiers = [
                    sopronElitMoziHuszarikTeremDefaultTier
                ]
            };

            var elitMozi = new Theater
            {
                Id = Guid.NewGuid(),
                Name = "Elit Mozi",
                Address = "Sopron, Torna u. 14, 9400 Hungary",
                Auditoriums = new List<Auditorium>()
                {
                    sopronElitMoziHuszarikTerem
                }
            };

            var sleepyHollowMovie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Sleepy Hollow",
                ReleaseYear = 1999,
                Description = "A movie about a headless horseman chopping other people´s heads off",
                Duration = TimeSpan.FromSeconds(105),
                Genres = new List<string> { "Dark Fantasy", "Slasher Horror", "Supernatural Horror", "Fantasy", "Horror", "Mistery" }
            };

            var sleepyHollowScreening = new Screening
            {
                Id = Guid.NewGuid(),
                AuditoriumId = sopronElitMoziHuszarikTerem.Id,
                MovieId = sleepyHollowMovie.Id,
                Showtime = DateTimeOffset.Now,
                Language = "English",
                Subtitles = "English"
            };

            var pricing = new Pricing
            {
                Id = Guid.NewGuid(),
                ScreeningId = sleepyHollowScreening.Id,
                TierId = sopronElitMoziHuszarikTeremDefaultTier.Id,
                Price = new Price
                {
                    Amount = 4500,
                    Currency = "HUF",
                }
            };

            var hansJuergenCustomer = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = "Hans Jürgen",
                LastName = "Waldmann",
                Email = "hans.juergen.waldmann@gmail.com"
            };

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                BookingState = BookingState.Confirmed,
                CustomerId = hansJuergenCustomer.Id,
                ScreeningId = sleepyHollowScreening.Id,
                CreatedOn = DateTimeOffset.UtcNow - TimeSpan.FromDays(3),
            };

            yield return
            [
                elitMozi,
                sleepyHollowMovie,
                sleepyHollowScreening,
                pricing,
                hansJuergenCustomer,
                booking
            ];
        }

        [Theory]
        [MemberData(nameof(AddSeatReservationsAsyncRejectsMultipleReservationsForTheSameSeatAsyncData))]
        public async Task AddSeatReservationsAsyncRejectsMultipleReservationsForTheSameSeatAsync(Theater theaterToSetup,
                                                                                                 Movie movieToSetup,
                                                                                                 Screening screeningToSetup,
                                                                                                 Pricing pricingToSetup,
                                                                                                 Customer customerToSetup,
                                                                                                 Booking bookingToSetup)
        {
            // Arrange
            await using var db = await CreateDatabaseAsync();
            var dbContext = db.Context;
            var seatReservationRepository = new SeatReservationRepository(mapper, databaseBinding, dbContext);
            {
                var theaterRepository = new TheaterRepository(mapper, dbContext);
                await theaterRepository.AddTheatersAsync([theaterToSetup]);

                var movieRepository = new MovieRepository(mapper, databaseBinding, dbContext);
                await movieRepository.AddMoviesAsync([movieToSetup]);

                var screeningRepository = new ScreeningRepository(mapper, dbContext);
                await screeningRepository.AddScreeningsAsync([screeningToSetup]);
                await screeningRepository.SetPricingAsync(pricingToSetup);

                var customerRepository = new CustomerRepository(mapper, dbContext);
                await customerRepository.AddCustomerAsync(customerToSetup);

                var bookingRepository = new BookingRepository(mapper, dbContext);
                await bookingRepository.AddBookingAsync(bookingToSetup);

                await dbContext.SaveChangesAsync();
            }
            var seatIdToReserve = theaterToSetup.Auditoriums[0].Tiers[0].Seats[0].Id;
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
            Assert.IsType<SeatReservationRepositoryException>(secondCallException);
            Assert.IsType<DbUpdateException>(secondCallException.InnerException);
            Assert.IsType<SqlException>(secondCallException.InnerException.InnerException);
            Assert.Null(secondCallException.InnerException.InnerException.InnerException);
            Assert.Equal("Could not reserve seats as at least one of them seems to be already reserved.", secondCallException.Message);
        }

        #endregion

        #region ReleaseSeatsOfTimeoutedBookingsAsync Tests

        public static IEnumerable<object[]> ReleaseSeatsOfTimeoutedBookingsAsyncDeletesTimeoutedSeatReservationsAsyncData()
        {
            var sopronElitMoziHuszarikTeremDefaultTier = new Tier
            {
                Id = Guid.NewGuid(),
                Name = "default",
                Seats = [
                    new Seat
                    {
                        Id = Guid.NewGuid(),
                        Row = 1,
                        Column = 1
                    },
                    new Seat
                    {
                        Id = Guid.NewGuid(),
                        Row = 2,
                        Column = 1
                    },
                    new Seat
                    {
                        Id = Guid.NewGuid(),
                        Row = 3,
                        Column = 1
                    },
                    new Seat
                    {
                        Id = Guid.NewGuid(),
                        Row = 4,
                        Column = 1
                    }
                ]
            };

            var sopronElitMoziHuszarikTerem = new Auditorium
            {
                Id = Guid.NewGuid(),
                Name = "Huszárik Terem",
                Tiers = [
                    sopronElitMoziHuszarikTeremDefaultTier
                ]
            };

            var elitMozi = new Theater
            {
                Id = Guid.NewGuid(),
                Name = "Elit Mozi",
                Address = "Sopron, Torna u. 14, 9400 Hungary",
                Auditoriums = new List<Auditorium>()
                {
                    sopronElitMoziHuszarikTerem
                }
            };

            var sleepyHollowMovie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Sleepy Hollow",
                ReleaseYear = 1999,
                Description = "A movie about a headless horseman chopping other people´s heads off",
                Duration = TimeSpan.FromSeconds(105),
                Genres = new List<string> { "Dark Fantasy", "Slasher Horror", "Supernatural Horror", "Fantasy", "Horror", "Mistery" }
            };

            var sleepyHollowScreening = new Screening
            {
                Id = Guid.NewGuid(),
                AuditoriumId = sopronElitMoziHuszarikTerem.Id,
                MovieId = sleepyHollowMovie.Id,
                Showtime = DateTimeOffset.Now,
                Language = "English",
                Subtitles = "English"
            };

            var pricing = new Pricing
            {
                Id = Guid.NewGuid(),
                ScreeningId = sleepyHollowScreening.Id,
                TierId = sopronElitMoziHuszarikTeremDefaultTier.Id,
                Price = new Price
                {
                    Amount = 4500,
                    Currency = "HUF",
                }
            };

            var hansJuergenCustomer = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = "Hans Jürgen",
                LastName = "Waldmann",
                Email = "hans.juergen.waldmann@gmail.com"
            };

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                BookingState = BookingState.ConfirmationTimeout,
                CustomerId = hansJuergenCustomer.Id,
                ScreeningId = sleepyHollowScreening.Id,
                CreatedOn = DateTimeOffset.UtcNow - TimeSpan.FromDays(3),
            };

            yield return
            [
                elitMozi,
                sleepyHollowMovie,
                sleepyHollowScreening,
                pricing,
                hansJuergenCustomer,
                booking
            ];
        }

        [Theory]
        [MemberData(nameof(ReleaseSeatsOfTimeoutedBookingsAsyncDeletesTimeoutedSeatReservationsAsyncData))]
        public async Task ReleaseSeatsOfTimeoutedBookingsAsyncDeletesTimeoutedSeatReservationsAsync(Theater theaterToSetup,
                                                                                                    Movie movieToSetup,
                                                                                                    Screening screeningToSetup,
                                                                                                    Pricing pricingToSetup,
                                                                                                    Customer customerToSetup,
                                                                                                    Booking bookingToSetup)
        {
            // Arrange
            await using var db = await CreateDatabaseAsync();
            var dbContext = db.Context;
            var seatReservationRepository = new SeatReservationRepository(mapper, databaseBinding, dbContext);
            {
                var theaterRepository = new TheaterRepository(mapper, dbContext);
                await theaterRepository.AddTheatersAsync([theaterToSetup]);

                var movieRepository = new MovieRepository(mapper, databaseBinding, dbContext);
                await movieRepository.AddMoviesAsync([movieToSetup]);

                var screeningRepository = new ScreeningRepository(mapper, dbContext);
                await screeningRepository.AddScreeningsAsync([screeningToSetup]);
                await screeningRepository.SetPricingAsync(pricingToSetup);

                var customerRepository = new CustomerRepository(mapper, dbContext);
                await customerRepository.AddCustomerAsync(customerToSetup);

                var bookingRepository = new BookingRepository(mapper, dbContext);
                await bookingRepository.AddBookingAsync(bookingToSetup);

                await dbContext.SaveChangesAsync();
            }
            var seatIdToReserve = theaterToSetup.Auditoriums[0].Tiers[0].Seats[0].Id;
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
