using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public interface IAddMoviesUseCase
{
    public Task ExecuteAsync(List<Movie> movies);
}
