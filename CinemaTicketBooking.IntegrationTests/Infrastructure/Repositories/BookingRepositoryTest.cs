using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.Repositories;

namespace CinemaTicketBooking.IntegrationTests.Infrastructure.Repositories
{
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

        public static IEnumerable<object[]> TimeoutUnconfirmedBookingsAsyncSetsBookingStateCorrectlyAsyncData()
        {
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
                hansJuergenCustomer,
                new Booking
                {
                    Id = Guid.NewGuid(),
                    BookingState = BookingState.Confirmed,
                    Customer = hansJuergenCustomer,
                    CreatedOn = DateTimeOffset.UtcNow - TimeSpan.FromDays(3),
                },
                BookingState.Confirmed
            ];

            yield return
            [
                2,
                hansJuergenCustomer,
                new Booking
                {
                    Id = Guid.NewGuid(),
                    BookingState = BookingState.NonConfirmed,
                    Customer = hansJuergenCustomer,
                    CreatedOn = DateTimeOffset.UtcNow - TimeSpan.FromDays(3),
                },
                BookingState.ConfirmationTimeout
            ];
        }

        [Theory]
        [MemberData(nameof(TimeoutUnconfirmedBookingsAsyncSetsBookingStateCorrectlyAsyncData))]
        async Task TimeoutUnconfirmedBookingsAsyncSetsBookingStateCorrectlyAsync(int timeoutInMinutes,
                                                                                 Customer customerToSetup,
                                                                                 Booking bookingToSetup,
                                                                                 BookingState expectedBookingState)
        {
            // Arrange
            await using var db = await CreateDatabaseAsync();
            var dbContext = db.Context;
            var customerRepository = new CustomerRepository(mapper, dbContext);
            await customerRepository.AddCustomerAsync(customerToSetup);
            var bookingRepository = new BookingRepository(mapper, dbContext);
            await bookingRepository.AddBookingAsync(bookingToSetup);
            await dbContext.SaveChangesAsync();
            var bookingId = bookingToSetup.Id.Value;

            // Act
            await bookingRepository.TimeoutUnconfirmedBookingsAsync(timeoutInMinutes);

            // Assert
            var booking = await bookingRepository.GetBookingAsync(bookingId);
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
