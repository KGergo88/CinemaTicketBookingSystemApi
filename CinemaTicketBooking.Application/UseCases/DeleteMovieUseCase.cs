using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;

namespace CinemaTicketBooking.Application.UseCases;

internal class DeleteMovieUseCase : IDeleteMovieUseCase
{
    private readonly IMovieRepository movieRepository;

    public DeleteMovieUseCase(IMovieRepository movieRepository)
    {
        this.movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
    }

    public async Task ExecuteAsync(IEnumerable<Guid> movieIdsToDelete)
    {
        try
        {
            await movieRepository.DeleteMoviesAsync(movieIdsToDelete);
        }
        catch (MovieRepositoryException ex)
        {
            throw new DeleteMoviesUseCaseException($"Could not delete movies. Details: {ex.Message}", ex);
        }
    }
}
