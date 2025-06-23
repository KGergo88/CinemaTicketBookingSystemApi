using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.Repositories;

public interface ISeatReservationRepository
{
    public Task AddSeatReservationsAsync(IEnumerable<SeatReservation> seatReservations);

    public Task DeleteSeatReservationsAsync(IEnumerable<Guid> seatReservationIds);

    public Task<List<Seat>> GetReservedSeatsOfTheScreeningAsync(Guid screeningId);

    public Task<List<SeatReservation>> GetSeatReservationsOfABookingAsync(Guid bookingId);

    public Task ReleaseSeatsOfTimeoutedBookingsAsync();

    public Task<List<Guid>> FindAlreadyReservedSeatIdsAsync(Guid screeningId, IEnumerable<Guid> seatIdsToCheck);
}
