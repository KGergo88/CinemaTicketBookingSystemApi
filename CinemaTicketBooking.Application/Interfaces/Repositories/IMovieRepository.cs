using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.Repositories;

public interface IMovieRepository
{
    public Task<List<Movie>> GetMoviesAsync();
}
