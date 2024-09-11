using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

public interface IGetMoviesUseCase
{
    public Task<List<Movie>> ExecuteAsync();
}
