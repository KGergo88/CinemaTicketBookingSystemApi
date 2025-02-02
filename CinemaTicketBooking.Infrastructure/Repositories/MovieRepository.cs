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
        var infraMovies = await context.Movies.Include(m => m.Genres)
                                              .ToListAsync();
        return mapper.Map<List<Domain.Entities.Movie>>(infraMovies);
    }

    public async Task AddMoviesAsync(List<Domain.Entities.Movie> domainMovies)
    {
        var infraMovies = new List<Infrastructure.Entities.MovieEntity>();
        var genresOfMovieIds = new Dictionary<Guid, List<string>>();

        foreach (var domainMovie in domainMovies)
        {
            var alreadyStoredMovie = await context.Movies.FirstOrDefaultAsync(m => m.Title == domainMovie.Title
                                                                                   && m.ReleaseYear == domainMovie.ReleaseYear);
            if (alreadyStoredMovie is not null)
            {
                throw new ArgumentException($"A movie with the Title \"{domainMovie.Title}\" from the year {domainMovie.ReleaseYear} is already stored!");
            }

            var infraMovie = mapper.Map<Infrastructure.Entities.MovieEntity>(domainMovie);
            infraMovies.Add(infraMovie);

            genresOfMovieIds.Add(infraMovie.Id, domainMovie.Genres);
        }

        var alreadyStoredGenres = await context.Genres.ToListAsync();
        var genresToCreate = genresOfMovieIds.Values.SelectMany(genres => genres)
                                                    .Distinct()
                                                    .Where(genre => !alreadyStoredGenres.Any(asg => asg.Name != genre))
                                                    .Select(genre => new GenreEntity { Name = genre })
                                                    .ToList();
        context.Genres.AddRange(genresToCreate);
        alreadyStoredGenres.AddRange(genresToCreate);

        infraMovies.ForEach(im => im.Genres = alreadyStoredGenres.Where(asg => genresOfMovieIds[im.Id].Contains(asg.Name))
                                                                 .ToList());
        context.AddRange(infraMovies);

        await context.SaveChangesAsync();
    }

    public async Task UpdateMovieAsync(Domain.Entities.Movie domainMovie)
    {
        var infraMovie = await context.Movies.Include(m => m.Genres)
                                             .Where(m => m.Id == domainMovie.Id)
                                             .SingleAsync();

        infraMovie.Title = domainMovie.Title;
        infraMovie.ReleaseYear = domainMovie.ReleaseYear;
        infraMovie.Description = domainMovie.Description;
        infraMovie.DurationInSeconds = (int)domainMovie.Duration.TotalSeconds;

        infraMovie.Genres.Clear();
        var alreadyStoredGenres = await context.Genres.ToListAsync();

        var genresToCreate = new List<GenreEntity>();
        foreach (var domainGenre in domainMovie.Genres)
        {
            var alreadyStoredGenre = alreadyStoredGenres.SingleOrDefault(asg => asg.Name == domainGenre);
            if (alreadyStoredGenre is not null)
            {
                infraMovie.Genres.Add(alreadyStoredGenre);
            }
            else
            {
                genresToCreate.Add(
                    new GenreEntity
                    {
                        Name = domainGenre,
                        Movies = [ infraMovie ]
                    }
                );
            }
        }
        context.Genres.AddRange(genresToCreate);

        await context.SaveChangesAsync();
    }

    public async Task DeleteMoviesAsync(List<Guid> movieIdsToDelete)
    {
        // Deleting this way and not via the ExecuteDeleteAsync()
        // as that solution would leave references to the deleted Movie
        // entitites in the Genre navigation properties.
        // This solution is not as fast as we need to make a DB round trip.
        var movies = await context.Movies.Where(m => movieIdsToDelete.Contains(m.Id))
                                         .ToListAsync();

        context.RemoveRange(movies);
        await context.SaveChangesAsync();
    }
}
