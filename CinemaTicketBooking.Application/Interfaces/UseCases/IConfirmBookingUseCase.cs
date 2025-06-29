namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public interface IConfirmBookingUseCase
{
    public Task ExecuteAsync(Guid bookingId);
}
