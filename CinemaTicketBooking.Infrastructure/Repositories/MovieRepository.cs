using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.Infrastructure.Repositories;

internal class MovieRepository : IMovieRepository
{
    private readonly IMapper mapper;
    private readonly CinemaTicketBookingDbContext context;

    public MovieRepository(IMapper mapper, CinemaTicketBookingDbContext context)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<Movie>> GetMoviesAsync()
    {
        var movies = await context.Movies.ToListAsync();
        return mapper.Map<List<Movie>>(movies);
    }
}
