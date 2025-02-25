using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

internal class UpdateMovieUseCase : IUpdateMovieUseCase
{
    private readonly IMovieRepository movieRepository;

    public UpdateMovieUseCase(IMovieRepository movieRepository)
    {
        this.movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
    }

    public async Task ExecuteAsync(Movie movie)
    {
        try
        {
            await movieRepository.UpdateMovieAsync(movie);
        }
        catch (MovieRepositoryException ex)
        {
            throw new UpdateMovieUseCaseException($"Could not update movie. Details: {ex.Message}", ex);
        }
    }
}
