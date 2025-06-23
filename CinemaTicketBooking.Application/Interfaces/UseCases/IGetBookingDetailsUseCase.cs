using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public class GetBookingDetailsException(string message, Exception? innerException = null)
    : UseCaseException(message, innerException) { }

public interface IGetBookingDetailsUseCase
{
    public Task<BookingDetails> ExecuteAsync(Guid bookingId);
}
