using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

internal class AddMoviesUseCase : IAddMoviesUseCase
{
    private readonly IMovieRepository movieRepository;

    public AddMoviesUseCase(IMovieRepository movieRepository)
    {
        this.movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
    }

    public async Task ExecuteAsync(IEnumerable<Movie> movies)
    {
        try
        {
            await movieRepository.AddMoviesAsync(movies);
        }
        catch (MovieRepositoryException ex)
        {
            throw new AddMoviesUseCaseException($"Could not add movies. Details: {ex.Message}", ex);
        }
    }
}
