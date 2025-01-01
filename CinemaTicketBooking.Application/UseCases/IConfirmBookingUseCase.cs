namespace CinemaTicketBooking.Application.UseCases;

public class ConfirmBookingException(string message) : Exception(message) {}

public interface IConfirmBookingUseCase
{
    public Task ExecuteAsync(Guid bookingId);
}
