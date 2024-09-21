using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

public interface IAddMoviesUseCase
{
    public Task ExecuteAsync(List<Movie> movies);
}
