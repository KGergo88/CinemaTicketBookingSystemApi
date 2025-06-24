using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.Repositories.Exceptions;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;

namespace CinemaTicketBooking.Application.UseCases;

internal class DeleteMovieUseCase : IDeleteMovieUseCase
{
    private readonly IMovieRepository movieRepository;

    public DeleteMovieUseCase(IMovieRepository movieRepository)
    {
        this.movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
    }

    public async Task ExecuteAsync(Guid movieIdToDelete)
    {
        try
        {
            await movieRepository.DeleteMoviesAsync([movieIdToDelete]);
        }
        catch(Interfaces.Repositories.Exceptions.NotFoundException ex)
        {
            throw new Interfaces.UseCases.Exceptions.NotFoundException($"No movie was found with the ID: {movieIdToDelete}", ex);
        }
        catch (RepositoryException ex)
        {
            throw new DeleteMovieUseCaseException($"Could not delete movie. Details: {ex.Message}", ex);
        }
    }
}
