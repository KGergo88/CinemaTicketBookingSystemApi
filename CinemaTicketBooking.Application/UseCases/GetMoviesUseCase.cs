using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

internal class GetMoviesUseCase : IGetMoviesUseCase
{
    private readonly IMovieRepository movieRepository;

    public GetMoviesUseCase(IMovieRepository movieRepository)
    {
        this.movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
    }

    public async Task<List<Movie>> ExecuteAsync()
    {
        var movies = await movieRepository.GetMoviesAsync();
        return movies;
    }
}
