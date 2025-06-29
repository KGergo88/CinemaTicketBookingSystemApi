using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public interface IMakeBookingUseCase
{
    public Task<Booking> ExecuteAsync(Guid customerId, Guid screeningId, IEnumerable<Guid> seatIdsToReserve);
}
