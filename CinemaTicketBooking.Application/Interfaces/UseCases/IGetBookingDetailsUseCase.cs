using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public interface IGetBookingDetailsUseCase
{
    public Task<BookingDetails> ExecuteAsync(Guid bookingId);
}
