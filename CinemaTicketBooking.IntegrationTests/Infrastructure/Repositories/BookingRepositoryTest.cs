using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.DatabaseBindings;
using CinemaTicketBooking.Infrastructure.Repositories;

namespace CinemaTicketBooking.IntegrationTests.Infrastructure.Repositories
{
    public class BookingRepositoryTest : TestDatabase
    {
        private readonly IMapper mapper;
        private readonly IDatabaseBinding databaseBinding;

        public BookingRepositoryTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            mapper = mappingConfig.CreateMapper();

            databaseBinding = DatabaseBindingFactory.Create("CinemaTicketBookingDbContext");
        }

        #region TimeoutUnconfirmedBookingsAsync Tests

        public static IEnumerable<object[]> TimeoutUnconfirmedBookingsAsyncSetsBookingStateCorrectlyAsyncData()
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

            yield return
            [
                2,
                elitMozi,
                sleepyHollowMovie,
                sleepyHollowScreening,
                pricing,
                hansJuergenCustomer,
                new Booking
                {
                    Id = Guid.NewGuid(),
                    BookingState = BookingState.Confirmed,
                    CustomerId = hansJuergenCustomer.Id,
                    ScreeningId = sleepyHollowScreening.Id,
                    CreatedOn = DateTimeOffset.UtcNow - TimeSpan.FromDays(3),
                },
                BookingState.Confirmed
            ];

            yield return
            [
                2,
                elitMozi,
                sleepyHollowMovie,
                sleepyHollowScreening,
                pricing,
                hansJuergenCustomer,
                new Booking
                {
                    Id = Guid.NewGuid(),
                    BookingState = BookingState.NonConfirmed,
                    CustomerId = hansJuergenCustomer.Id,
                    ScreeningId = sleepyHollowScreening.Id,
                    CreatedOn = DateTimeOffset.UtcNow - TimeSpan.FromDays(3),
                },
                BookingState.ConfirmationTimeout
            ];
        }

        [Theory]
        [MemberData(nameof(TimeoutUnconfirmedBookingsAsyncSetsBookingStateCorrectlyAsyncData))]
        async Task TimeoutUnconfirmedBookingsAsyncSetsBookingStateCorrectlyAsync(int timeoutInMinutes,
                                                                                 Theater theaterToSetup,
                                                                                 Movie movieToSetup,
                                                                                 Screening screeningToSetup,
                                                                                 Pricing pricingToSetup,
                                                                                 Customer customerToSetup,
                                                                                 Booking bookingToSetup,
                                                                                 BookingState expectedBookingState)
        {
            // Arrange
            await using var db = await CreateDatabaseAsync();
            var dbContext = db.Context;
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
            var bookingId = bookingToSetup.Id;

            // Act
            await bookingRepository.TimeoutUnconfirmedBookingsAsync(timeoutInMinutes);

            // Assert
            var booking = await bookingRepository.GetBookingOrNullAsync(bookingId);
            Assert.Equal(expectedBookingState, booking.BookingState);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        async Task TimeoutUnconfirmedBookingsAsyncThrowsForInvalidTimeoutLimits(int timeoutInMinutes)
        {
            // Arrange
            await using var db = await CreateDatabaseAsync();
            var dbContext = db.Context;
            var bookingRepository = new BookingRepository(mapper, dbContext);

            // Act
            var exception = await Record.ExceptionAsync(async () => await bookingRepository.TimeoutUnconfirmedBookingsAsync(timeoutInMinutes));

            // Assert
            Assert.IsType<BookingRepositoryException>(exception);
            Assert.Equal($"{nameof(timeoutInMinutes)} shall be greater than zero! Actual value: {timeoutInMinutes}", exception.Message);
        }
        #endregion
    }
}
