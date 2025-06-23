using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public class ConfirmBookingException(string message, Exception? innerException = null)
    : UseCaseException(message, innerException) { }

public interface IConfirmBookingUseCase
{
    public Task ExecuteAsync(Guid bookingId);
}
