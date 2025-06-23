using CinemaTicketBooking.Application.Interfaces.Repositories.Exceptions;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.Repositories;

public class MovieRepositoryException(string message, Exception? innerException = null)
    : RepositoryException(message, innerException) { }

public interface IMovieRepository
{
    public Task<Movie> GetMovieOrNullAsync(Guid movieId);

    public Task<List<Movie>> GetMoviesAsync();

    public Task AddMoviesAsync(IEnumerable<Movie> domainMovies);

    public Task UpdateMovieAsync(Movie domainMovie);

    public Task DeleteMoviesAsync(IEnumerable<Guid> movieIdsToDelete);
}
