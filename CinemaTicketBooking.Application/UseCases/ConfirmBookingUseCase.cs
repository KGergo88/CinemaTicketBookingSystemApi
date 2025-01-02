using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

internal class ConfirmBookingUseCase
{
    private readonly IBookingRepository bookingRepository;

    public ConfirmBookingUseCase(IBookingRepository bookingRepository)
    {
        this.bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
    }

    public async Task ExecuteAsync(Guid bookingId)
    {
        if (bookingId == Guid.Empty)
            throw new ConfirmBookingException($"Invalid booking ID: {bookingId}");

        var booking = await bookingRepository.GetBookingAsync(bookingId);

        if (booking == null)
            throw new ConfirmBookingException("Booking not found!");

        if (booking.BookingState != BookingState.NonConfirmed)
            throw new ConfirmBookingException("Booking is not confirmable!");

        booking.BookingState = BookingState.Confirmed;
        await bookingRepository.UpdateBookingAsync(booking);
    }
}
