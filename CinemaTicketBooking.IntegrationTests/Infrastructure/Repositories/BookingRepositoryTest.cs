using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.DatabaseSeeding;
using CinemaTicketBooking.Infrastructure.Repositories;

namespace CinemaTicketBooking.IntegrationTests.Infrastructure.Repositories
{
    [Trait("Category", "LocalDbBasedTests")]
    public class BookingRepositoryTest : TestDatabase
    {
        private readonly IMapper mapper;

        public BookingRepositoryTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            mapper = mappingConfig.CreateMapper();
        }

        #region TimeoutUnconfirmedBookingsAsync Tests

        static (Screening, Pricing, Booking) CreateTestEntities(SeedData seedData, BookingState bookingStateToSetup)
        {
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
                BookingState = bookingStateToSetup,
                CustomerId = customer.Id,
                ScreeningId = screening.Id,
                CreatedOn = DateTimeOffset.UtcNow - TimeSpan.FromDays(3),
            };

            return (screening, pricing, booking);
        }

        public static IEnumerable<object[]> TimeoutUnconfirmedBookingsAsyncSetsBookingStateCorrectlyAsyncData()
        {
            // The seed data needs to be created new for each test run as otherwise they would get contaminated
            // by the previous test runs, which would lead to false positives or negatives.
            // The concrete issue would be that there would be a screening entity referenced by one of the tiers.
            // Being able to use a static SeedData in the test class would lead to more readable tests, but we cannot do that
            // because the tests need to be isolated from each other. Not having a static seed data is not possible as methods used
            // in MemberDataAttribute require a static method or property to return the data.
            var result = new TheoryData<SeedData, Screening, Pricing, Booking, BookingState>();

            {
                var seedData = new DefaultSeedData();
                var (screening, pricing, booking) = CreateTestEntities(seedData, BookingState.Confirmed);
                result.Add(seedData,screening, pricing, booking, BookingState.Confirmed);
            }
            {
                var seedData = new DefaultSeedData();
                var (screening, pricing, booking) = CreateTestEntities(seedData, BookingState.NonConfirmed);
                result.Add(seedData, screening, pricing, booking, BookingState.ConfirmationTimeout);
            }

            return result;
        }

        [Theory]
        [MemberData(nameof(TimeoutUnconfirmedBookingsAsyncSetsBookingStateCorrectlyAsyncData))]
        async Task TimeoutUnconfirmedBookingsAsyncSetsBookingStateCorrectlyAsync(SeedData seedData,
                                                                                 Screening screeningToSetup,
                                                                                 Pricing pricingToSetup,
                                                                                 Booking bookingToSetup,
                                                                                 BookingState expectedBookingState)
        {
            // Arrange
            var timeoutInMinutes = 2;
            await using var db = await CreateDatabaseAsync(seedData);
            var screeningRepository = new ScreeningRepository(mapper, db.Context);
            var bookingRepository = new BookingRepository(mapper, db.Context);
            await screeningRepository.AddScreeningsAsync([screeningToSetup]);
            await screeningRepository.SetPricingAsync(pricingToSetup);
            await bookingRepository.AddBookingAsync(bookingToSetup);

            // Act
            await bookingRepository.TimeoutUnconfirmedBookingsAsync(timeoutInMinutes);

            // Assert
            var storedBooking = await bookingRepository.GetBookingOrNullAsync(bookingToSetup.Id);
            Assert.NotNull(storedBooking);
            Assert.Equal(expectedBookingState, storedBooking.BookingState);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task TimeoutUnconfirmedBookingsAsyncThrowsForInvalidTimeoutLimits(int timeoutInMinutes)
        {
            // Arrange
            await using var db = await CreateDatabaseAsync();
            var bookingRepository = new BookingRepository(mapper, db.Context);

            // Act
            var exception = await Record.ExceptionAsync(async () => await bookingRepository.TimeoutUnconfirmedBookingsAsync(timeoutInMinutes));

            // Assert
            Assert.IsType<BookingRepositoryException>(exception);
            Assert.Equal($"{nameof(timeoutInMinutes)} shall be greater than zero! Actual value: {timeoutInMinutes}", exception.Message);
        }

        #endregion
    }
}
