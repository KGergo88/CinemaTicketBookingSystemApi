using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Infrastructure.Entities;
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

    public async Task<List<Domain.Entities.Movie>> GetMoviesAsync()
    {
        var infraMovies = await context.Movies
                                       .Include(m => m.Genres)
                                       .ToListAsync();
        return mapper.Map<List<Domain.Entities.Movie>>(infraMovies);
    }

    public async Task AddMoviesAsync(List<Domain.Entities.Movie> domainMovies)
    {
        var infraMovies = new List<Infrastructure.Entities.Movie>();
        var genresOfMovieIds = new Dictionary<Guid, List<string>>();

        foreach (var domainMovie in domainMovies)
        {
            var alreadyStoredMovie = await context.Movies.FirstOrDefaultAsync(m => m.Title == domainMovie.Title
                                                                                   && m.ReleaseYear == domainMovie.ReleaseYear);
            if (alreadyStoredMovie is not null)
            {
                throw new ArgumentException($"A movie with the Title \"{domainMovie.Title}\" from the year {domainMovie.ReleaseYear} is already stored!");
            }

            var infraMovie = mapper.Map<Infrastructure.Entities.Movie>(domainMovie);
            infraMovie.Id = Guid.NewGuid();

            genresOfMovieIds.Add(infraMovie.Id, domainMovie.Genres);
            infraMovies.Add(infraMovie);
        }

        var alreadyStoredGenres = await context.Genres.ToListAsync();
        var genresToCreate = genresOfMovieIds.Values.SelectMany(genres => genres)
                                                    .Distinct()
                                                    .Where(genre => !alreadyStoredGenres.Any(asg => asg.Name != genre))
                                                    .Select(genre => new Genre { Name = genre })
                                                    .ToList();
        context.Genres.AddRange(genresToCreate);
        alreadyStoredGenres.AddRange(genresToCreate);

        infraMovies.ForEach(im => im.Genres = alreadyStoredGenres.Where(asg => genresOfMovieIds[im.Id].Contains(asg.Name))
                                                                 .ToList());
        context.AddRange(infraMovies);

        await context.SaveChangesAsync();
    }
}
