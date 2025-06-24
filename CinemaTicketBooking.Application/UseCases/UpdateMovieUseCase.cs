using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.Repositories.Exceptions;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;
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
        catch (Interfaces.Repositories.Exceptions.NotFoundException ex)
        {
            throw new Interfaces.UseCases.Exceptions.NotFoundException($"No movie was found with the ID: {movie.Id}", ex);
        }
        catch (RepositoryException ex)
        {
            throw new UpdateMovieUseCaseException($"Could not update movie. Details: {ex.Message}", ex);
        }
    }
}
