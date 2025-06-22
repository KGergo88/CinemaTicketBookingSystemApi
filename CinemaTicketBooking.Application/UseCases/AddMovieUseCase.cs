using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

internal class AddMovieUseCase : IAddMovieUseCase
{
    private readonly IMovieRepository movieRepository;

    public AddMovieUseCase(IMovieRepository movieRepository)
    {
        this.movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
    }

    public async Task ExecuteAsync(Movie movie)
    {
        try
        {
            await movieRepository.AddMoviesAsync([movie]);
        }
        catch (MovieRepositoryException ex)
        {
            throw new AddMovieUseCaseException($"Could not add movie. Details: {ex.Message}", ex);
        }
    }
}
