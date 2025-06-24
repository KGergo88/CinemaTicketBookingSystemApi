using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public interface IAddMovieUseCase
{
    public Task ExecuteAsync(Movie movies);
}
