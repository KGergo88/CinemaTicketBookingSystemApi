using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.Repositories;

public interface ITheaterRepository
{
    public Task AddTheatersAsync(List<Theater> domainTheaters);

    public Task<Theater> GetTheaterOfAScreeningAsync(Guid screeningId);
}
