using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

public class MakeBookingException(string message) : Exception(message) {}

public interface IMakeBookingUseCase
{
    public Task<Booking> ExecuteAsync(Guid customerId, Guid screeningId, List<Guid> seatsToReserve);
}
