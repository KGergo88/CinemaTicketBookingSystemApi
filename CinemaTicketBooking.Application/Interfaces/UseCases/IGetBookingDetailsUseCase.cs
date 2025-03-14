using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public class GetBookingDetailsException(string message, Exception? innerException = null)
    : Exception(message, innerException) { }

public interface IGetBookingDetailsUseCase
{
    public Task<BookingDetails> ExecuteAsync(Guid bookingId);
}
