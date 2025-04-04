using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.Repositories;

public class SeatReservationRepositoryException(string message, Exception? innerException = null)
    : Exception(message, innerException) { }

public interface ISeatReservationRepository
{
    public Task AddSeatReservationsAsync(IEnumerable<Guid> seatIdsToReserve, Guid bookingId, Guid screeningId);

    public Task<List<Seat>> GetReservedSeatsOfTheScreeningAsync(Guid screeningId);

    public Task<List<SeatReservation>> GetSeatReservationsOfABookingAsync(Guid bookingId);

    public Task ReleaseSeatsOfTimeoutedBookingsAsync();
}
