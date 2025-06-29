using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public interface IUpdateMovieUseCase
{
    public Task ExecuteAsync(Movie movie);
}
