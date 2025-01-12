using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public class GetBookingDetailsException(string message) : Exception(message) {}

public interface IGetBookingDetailsUseCase
{
    public Task<BookingDetails> ExecuteAsync(Guid bookingId);
}
