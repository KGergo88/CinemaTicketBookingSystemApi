using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.Repositories;

public interface ITheaterRepository
{
    public Task AddTheatersAsync(IEnumerable<Theater> domainTheaters);

    public Task<Theater> GetTheaterOfAScreeningAsync(Guid screeningId);

    public Task<Auditorium> GetAuditoriumOrNullAsync(Guid auditoriumId);

    public Task<Tier> GetTierOrNullAsync(Guid tierId);
}
