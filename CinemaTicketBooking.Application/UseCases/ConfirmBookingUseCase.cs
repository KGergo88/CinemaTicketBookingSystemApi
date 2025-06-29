using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

internal class ConfirmBookingUseCase : IConfirmBookingUseCase
{
    private readonly IBookingRepository bookingRepository;

    public ConfirmBookingUseCase(IBookingRepository bookingRepository)
    {
        this.bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
    }

    public async Task ExecuteAsync(Guid bookingId)
    {
        if (bookingId == Guid.Empty)
            throw new UseCaseException($"Invalid booking ID: {bookingId}");

        var booking = await bookingRepository.GetBookingOrNullAsync(bookingId);

        if (booking == null)
            throw new NotFoundException("Booking not found!");

        if (booking.BookingState != BookingState.NonConfirmed)
            throw new ConflictException("Booking is not confirmable!");

        await bookingRepository.SetBookingStateAsync(bookingId, BookingState.Confirmed);
    }
}
