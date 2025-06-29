using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.Repositories.Exceptions;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;
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
        catch (DuplicateException ex)
        {
            throw new ConflictException($"This movie already exists!", ex);
        }
        catch (RepositoryException ex)
        {
            throw new UseCaseException($"Could not add movie. Details: {ex.Message}", ex);
        }
    }
}
