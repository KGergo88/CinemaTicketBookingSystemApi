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
                Auditorium = sopronElitMoziHuszarikTerem,
                Movie = sleepyHollowMovie,
                Showtime = DateTimeOffset.Now,
                Language = "English",
                Subtitles = "English"
            };

            var pricing = new Pricing
            {
                Id = Guid.NewGuid(),
                Screening = sleepyHollowScreening,
                Tier = sopronElitMoziHuszarikTeremDefaultTier,
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
                Customer = hansJuergenCustomer,
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
        async Task AddSeatReservationsAsyncRejectsMultipleReservationsForTheSameSeatAsync(Theater theaterToSetup,
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

                var movieRepository = new MovieRepository(mapper, dbContext);
                await movieRepository.AddMoviesAsync([movieToSetup]);

                var screeningRepository = new ScreeningRepository(mapper, dbContext);
                await screeningRepository.AddScreeningsAsync([screeningToSetup]);
                await screeningRepository.SetPricingAsync(screeningToSetup.Id.Value, theaterToSetup.Auditoriums[0].Tiers[0].Id.Value, pricingToSetup);

                var customerRepository = new CustomerRepository(mapper, dbContext);
                await customerRepository.AddCustomerAsync(customerToSetup);

                var bookingRepository = new BookingRepository(mapper, dbContext);
                await bookingRepository.AddBookingAsync(bookingToSetup);

                await dbContext.SaveChangesAsync();
            }
            var seatToReserve = theaterToSetup.Auditoriums[0].Tiers[0].Seats[0].Id.Value;
            var bookingId = bookingToSetup.Id.Value;
            var screeningId = screeningToSetup.Id.Value;

            // Act
            var firstCallException = await Record.ExceptionAsync(
                () => seatReservationRepository.AddSeatReservationsAsync([seatToReserve], bookingId, screeningId));
            var secondCallException = await Record.ExceptionAsync(
                () => seatReservationRepository.AddSeatReservationsAsync([seatToReserve], bookingId, screeningId));

            // Assert
            Assert.Null(firstCallException);
            Assert.IsType<SeatReservationRepositoryException>(secondCallException);
            Assert.IsType<DbUpdateException>(secondCallException.InnerException);
            Assert.IsType<SqlException>(secondCallException.InnerException.InnerException);
            Assert.Null(secondCallException.InnerException.InnerException.InnerException);
            Assert.Equal("Could not reserve seats as at least one of them seems to be already reserved.", secondCallException.Message);
        }

        #endregion

        #region GetAvailableSeatsAsync Tests

        public static IEnumerable<object[]> GetAvailableSeatsReturnsTheAvailableSeatsAsyncData()
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
                Auditorium = sopronElitMoziHuszarikTerem,
                Movie = sleepyHollowMovie,
                Showtime = DateTimeOffset.Now,
                Language = "English",
                Subtitles = "English"
            };

            var pricing = new Pricing
            {
                Id = Guid.NewGuid(),
                Screening = sleepyHollowScreening,
                Tier = sopronElitMoziHuszarikTeremDefaultTier,
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
                Customer = hansJuergenCustomer,
                CreatedOn = DateTimeOffset.UtcNow - TimeSpan.FromDays(3),
            };

            var seatIdsToReserve = new List<Guid>() {
                sopronElitMoziHuszarikTeremDefaultTier.Seats[0].Id.Value,
                sopronElitMoziHuszarikTeremDefaultTier.Seats[1].Id.Value,
            };

            var expectedAvailableSeats = new List<Seat>() {
                sopronElitMoziHuszarikTeremDefaultTier.Seats[2],
                sopronElitMoziHuszarikTeremDefaultTier.Seats[3],
            };

            yield return
            [
                elitMozi,
                sleepyHollowMovie,
                sleepyHollowScreening,
                pricing,
                hansJuergenCustomer,
                booking,
                seatIdsToReserve,
                expectedAvailableSeats
            ];
        }

        [Theory]
        [MemberData(nameof(GetAvailableSeatsReturnsTheAvailableSeatsAsyncData))]
        async Task GetAvailableSeatsReturnsTheAvailableSeatsAsync(Theater theaterToSetup,
                                                                  Movie movieToSetup,
                                                                  Screening screeningToSetup,
                                                                  Pricing pricingToSetup,
                                                                  Customer customerToSetup,
                                                                  Booking bookingToSetup,
                                                                  List<Guid> seatIdsToReserve,
                                                                  List<Seat> expectedAvailableSeats)
        {
            // Arrange
            await using var db = await CreateDatabaseAsync();
            var dbContext = db.Context;
            var seatReservationRepository = new SeatReservationRepository(mapper, databaseBinding, dbContext);
            {
                var theaterRepository = new TheaterRepository(mapper, dbContext);
                await theaterRepository.AddTheatersAsync([theaterToSetup]);

                var movieRepository = new MovieRepository(mapper, dbContext);
                await movieRepository.AddMoviesAsync([movieToSetup]);

                var screeningRepository = new ScreeningRepository(mapper, dbContext);
                await screeningRepository.AddScreeningsAsync([screeningToSetup]);
                await screeningRepository.SetPricingAsync(screeningToSetup.Id.Value, theaterToSetup.Auditoriums[0].Tiers[0].Id.Value, pricingToSetup);

                var customerRepository = new CustomerRepository(mapper, dbContext);
                await customerRepository.AddCustomerAsync(customerToSetup);

                var bookingRepository = new BookingRepository(mapper, dbContext);
                await bookingRepository.AddBookingAsync(bookingToSetup);

                await dbContext.SaveChangesAsync();
            }
            var bookingId = bookingToSetup.Id.Value;
            var screeningId = screeningToSetup.Id.Value;
            await seatReservationRepository.AddSeatReservationsAsync(seatIdsToReserve, bookingId, screeningId);

            // Act
            var availableSeats = await seatReservationRepository.GetAvailableSeatsAsync(screeningId);

            // Assert
            Assert.Equivalent(expectedAvailableSeats, availableSeats);
        }

        #endregion
    }
}
