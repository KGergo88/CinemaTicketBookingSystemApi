using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public class MakeBookingException(string message, Exception? innerException = null)
    : Exception(message, innerException) { }

public interface IMakeBookingUseCase
{
    public Task<Booking> ExecuteAsync(Guid customerId, Guid screeningId, IEnumerable<Guid> seatIdsToReserve);
}
