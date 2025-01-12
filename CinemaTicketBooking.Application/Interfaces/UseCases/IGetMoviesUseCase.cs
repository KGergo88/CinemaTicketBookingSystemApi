using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public interface IGetMoviesUseCase
{
    public Task<List<Movie>> ExecuteAsync();
}
