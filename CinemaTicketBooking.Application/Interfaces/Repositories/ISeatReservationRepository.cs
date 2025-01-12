using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.Repositories;

public class SeatReservationRepositoryException(string message, Exception? innerException = null)
    : Exception(message, innerException) { }

public interface ISeatReservationRepository
{
    public Task AddSeatReservationsAsync(List<Guid> seatIdsToReserve, Guid bookingId, Guid screeningId);

    public Task<List<Seat>> GetAvailableSeats(Guid screningId);

    public Task<List<SeatReservation>> GetSeatReservationsOfABookingAsync(Guid bookingId);
}
