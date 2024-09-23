using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

public interface IUpdateMovieUseCase
{
    public Task ExecuteAsync(Movie movie);
}
