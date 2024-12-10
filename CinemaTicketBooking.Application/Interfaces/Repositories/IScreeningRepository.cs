using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.Repositories;

public interface IScreeningRepository
{
    public Task AddScreeningsAsync(List<Screening> domainScreenings);
}
