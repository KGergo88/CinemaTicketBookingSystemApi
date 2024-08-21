using AutoMapper;
using CinemaTicketBooking.Application.Dtos;
using CinemaTicketBooking.Application.Interfaces.Repositories;

namespace CinemaTicketBooking.Application.UseCases;

internal class GetMoviesUseCase : IGetMoviesUseCase
{
    private readonly IMapper mapper;
    private readonly IMovieRepository movieRepository;

    public GetMoviesUseCase(IMapper mapper, IMovieRepository movieRepository)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
    }

    public async Task<List<MovieDto>> ExecuteAsync()
    {
        var movies = await movieRepository.GetMoviesAsync();
        return mapper.Map<List<MovieDto>>(movies);
    }
}
