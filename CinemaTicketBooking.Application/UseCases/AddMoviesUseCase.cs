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
        await movieRepository.AddMoviesAsync(movies);
    }
}
