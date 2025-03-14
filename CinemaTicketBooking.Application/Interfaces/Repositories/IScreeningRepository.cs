using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.Repositories;

public interface IScreeningRepository
{
    public Task<Screening?> GetScreeningOrNullAsync(Guid id);

    public Task AddScreeningsAsync(IEnumerable<Screening> domainScreenings);

    public Task SetPricingAsync(Pricing pricing);

    public Task<List<Seat>> GetAllSeatsOfTheScreeningAsync(Guid screeningId);
}
