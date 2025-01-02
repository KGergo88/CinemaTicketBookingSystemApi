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
        await movieRepository.UpdateMovieAsync(movie);
    }
}
