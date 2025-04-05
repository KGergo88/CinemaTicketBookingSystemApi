using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Application.UseCases;
using CinemaTicketBooking.Domain.Entities;
using Moq;

namespace CinemaTicketBooking.Application.UnitTests.UseCases;

public class ConfirmBookingUseCaseTest
{
    private readonly Mock<IBookingRepository> mockBookingRepository = new();

    #region ExecuteAsync Tests

    [Fact]
    public async Task BookingWithEmptyIdCannotBeConfirmedAsync()
    {
        // Arrange
        var confirmBookingUseCase = new ConfirmBookingUseCase(mockBookingRepository.Object);
        var bookingId = Guid.Empty;

        // Act
        var exception = await Record.ExceptionAsync(
            () => confirmBookingUseCase.ExecuteAsync(bookingId)
        );

        // Assert
        Assert.IsType<ConfirmBookingException>(exception);
        Assert.Equal($"Invalid booking ID: {bookingId}", exception.Message);
    }

    [Fact]
    public async Task NonExistingBookingCannotBeConfirmedAsync()
    {
        // Arrange
        mockBookingRepository.Setup(
            mbr => mbr.GetBookingAsync(
                It.IsAny<Guid>())).ReturnsAsync(null as Booking);
        var confirmBookingUseCase = new ConfirmBookingUseCase(mockBookingRepository.Object);
        var bookingId = Guid.NewGuid();

        // Act
        var exception = await Record.ExceptionAsync(
            () => confirmBookingUseCase.ExecuteAsync(bookingId)
        );

        // Assert
        Assert.IsType<ConfirmBookingException>(exception);
        Assert.Equal("Booking not found!", exception.Message);
    }

    public static IEnumerable<object[]> OnlyNonConfirmedBookingCanBeConfirmedAsyncData()
    {
        foreach (var bookingState in Enum.GetValues<BookingState>())
        {
            yield return new object[] { bookingState };
        }
    }

    [Theory]
    [MemberData(nameof(OnlyNonConfirmedBookingCanBeConfirmedAsyncData))]
    public async Task OnlyNonConfirmedBookingCanBeConfirmedAsync(BookingState bookingState)
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var screeningId = Guid.NewGuid();
        var mockBooking = new Booking()
        {
            Id = bookingId,
            BookingState = bookingState,
            CustomerId = Guid.NewGuid(),
            ScreeningId = screeningId,
            CreatedOn = DateTimeOffset.Now
        };
        mockBookingRepository.Setup(
            mbr => mbr.GetBookingAsync(
                It.IsAny<Guid>())).ReturnsAsync(mockBooking);
        var confirmBookingUseCase = new ConfirmBookingUseCase(mockBookingRepository.Object);

        // Act
        var exception = await Record.ExceptionAsync(
            () => confirmBookingUseCase.ExecuteAsync(bookingId)
        );

        // Assert
        if (bookingState == BookingState.NonConfirmed)
        {
            Assert.Null(exception);
            mockBookingRepository.Verify(
                mbr => mbr.UpdateBookingAsync(
                    It.Is<Booking>(b => b.Id == bookingId
                                        && b.BookingState == BookingState.Confirmed
                                        && b.CustomerId == mockBooking.CustomerId
                                        && b.ScreeningId == screeningId
                                        && b.CreatedOn == mockBooking.CreatedOn)),
                Times.Once);
        }
        else
        {
            Assert.IsType<ConfirmBookingException>(exception);
            Assert.Equal("Booking is not confirmable!", exception.Message);
        }
    }

    #endregion
}
