namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public class ConfirmBookingException(string message, Exception? innerException = null)
    : Exception(message, innerException) { }

public interface IConfirmBookingUseCase
{
    public Task ExecuteAsync(Guid bookingId);
}
