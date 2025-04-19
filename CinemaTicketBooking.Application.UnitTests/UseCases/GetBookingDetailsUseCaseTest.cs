﻿using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Application.UseCases;
using CinemaTicketBooking.Domain.Entities;
using Moq;

namespace CinemaTicketBooking.Application.UnitTests.UseCases;

public class GetBookingDetailsUseCaseTest
{
    private readonly Mock<IBookingRepository> mockBookingRepository = new();
    private readonly Mock<ITheaterRepository> mockTheaterRepository = new();
    private readonly Mock<IScreeningRepository> mockScreeningRepository = new();
    private readonly Mock<ISeatReservationRepository> mockSeatReservationRepository = new();

    #region ExecuteAsync Tests

    [Fact]
    public async Task UnknownBookingsAreRejectedAsync()
    {
        // Arrange
        var getBookingDetailsUseCase = new GetBookingDetailsUseCase(mockBookingRepository.Object,
                                                                    mockTheaterRepository.Object,
                                                                    mockScreeningRepository.Object,
                                                                    mockSeatReservationRepository.Object);

        // Act
        var bookingId = Guid.NewGuid();
        var exception = await Record.ExceptionAsync(
            () => getBookingDetailsUseCase.ExecuteAsync(bookingId)
        );

        // Assert
        Assert.IsType<GetBookingDetailsException>(exception);
        Assert.Equal($"Booking not found! Id: {bookingId}", exception.Message);
    }

    [Fact]
    public async Task BookingMustHaveSeatReservationsAsync()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        mockBookingRepository.Setup(
            mbr => mbr.GetBookingAsync(bookingId))
            .ReturnsAsync(
                new Booking
                {
                    Id = bookingId,
                    BookingState = BookingState.Confirmed,
                    CustomerId = Guid.NewGuid(),
                    ScreeningId = Guid.NewGuid(),
                    CreatedOn = DateTimeOffset.UtcNow
                }
            );
        mockSeatReservationRepository.Setup(
            msrr => msrr.GetSeatReservationsOfABookingAsync(bookingId))
            .ReturnsAsync(
                []
            );
        var getBookingDetailsUseCase = new GetBookingDetailsUseCase(mockBookingRepository.Object,
                                                                    mockTheaterRepository.Object,
                                                                    mockScreeningRepository.Object,
                                                                    mockSeatReservationRepository.Object);

        // Act
        var exception = await Record.ExceptionAsync(
            () => getBookingDetailsUseCase.ExecuteAsync(bookingId)
        );

        // Assert
        Assert.IsType<GetBookingDetailsException>(exception);
        Assert.Equal($"There are no seat reservations for this booking! BookingId: {bookingId}", exception.Message);
    }

    [Fact]
    public async Task SeatReservationsMustContainTheScreeningIdAsync()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var screeningId = Guid.NewGuid();
        mockBookingRepository.Setup(
            mbr => mbr.GetBookingOrNullAsync(bookingId))
            .ReturnsAsync(
                new Booking
                {
                    Id = bookingId,
                    BookingState = BookingState.Confirmed,
                    CustomerId = Guid.NewGuid(),
                    ScreeningId = screeningId,
                    CreatedOn = DateTimeOffset.UtcNow
                }
            );
        mockSeatReservationRepository.Setup(
            msrr => msrr.GetSeatReservationsOfABookingAsync(bookingId))
            .ReturnsAsync(
                [
                    new SeatReservation
                    {
                        Id = Guid.NewGuid(),
                        BookingId = Guid.NewGuid(),
                        ScreeningId = screeningId,
                        SeatId = Guid.NewGuid(),
                        Price = null
                    }
                ]
            );
        var getBookingDetailsUseCase = new GetBookingDetailsUseCase(mockBookingRepository.Object,
                                                                    mockTheaterRepository.Object,
                                                                    mockScreeningRepository.Object,
                                                                    mockSeatReservationRepository.Object);

        // Act
        var exception = await Record.ExceptionAsync(
            () => getBookingDetailsUseCase.ExecuteAsync(bookingId)
        );

        // Assert
        Assert.IsType<GetBookingDetailsException>(exception);
        Assert.Equal($"The screening of the booking does not exist! ScreeningId: {screeningId} BookingId: {bookingId}", exception.Message);
    }

    #endregion
}
