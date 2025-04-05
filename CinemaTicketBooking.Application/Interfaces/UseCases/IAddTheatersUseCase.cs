using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public interface IAddTheatersUseCase
{
    public Task ExecuteAsync(IEnumerable<Theater> theaters);
}
