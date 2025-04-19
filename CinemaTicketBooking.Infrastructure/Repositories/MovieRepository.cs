using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Infrastructure.DatabaseBindings;
using CinemaTicketBooking.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.Infrastructure.Repositories;

internal class MovieRepository : IMovieRepository
{
    private readonly IMapper mapper;
    private readonly IDatabaseBinding databaseBinding;
    private readonly CinemaTicketBookingDbContext context;

    public MovieRepository(IMapper mapper, IDatabaseBinding databaseBinding, CinemaTicketBookingDbContext context)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.databaseBinding = databaseBinding ?? throw new ArgumentNullException(nameof(databaseBinding));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Movie> GetMovieOrNullAsync(Guid movieId)
    {
        var infraMovie = await context.Movies.Include(m => m.Genres)
                                             .SingleOrDefaultAsync(m => m.Id == movieId);

        return mapper.Map<Movie>(infraMovie);
    }

    public async Task<List<Movie>> GetMoviesAsync()
    {
        var infraMovies = await context.Movies.Include(m => m.Genres)
                                              .ToListAsync();
        return mapper.Map<List<Movie>>(infraMovies);
    }

    public async Task AddMoviesAsync(IEnumerable<Movie> domainMovies)
    {
        var infraMovies = new List<MovieEntity>();
        var genresOfMovieIds = new Dictionary<Guid, List<string>>();

        foreach (var domainMovie in domainMovies)
        {
            var alreadyStoredMovie = await context.Movies.FirstOrDefaultAsync(m => m.Title == domainMovie.Title
                                                                                   && m.ReleaseYear == domainMovie.ReleaseYear);
            if (alreadyStoredMovie is not null)
                throw new MovieRepositoryException(
                    $"A movie with the Title \"{domainMovie.Title}\" from the year {domainMovie.ReleaseYear} is already stored!");

            var infraMovie = mapper.Map<MovieEntity>(domainMovie);
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

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            if (databaseBinding.IsUniqueIndexException(ex))
                throw new MovieRepositoryException($"Cannot add the same movie twice!", ex);

            throw;
        }
    }

    public async Task UpdateMovieAsync(Movie domainMovie)
    {
        MovieEntity? infraMovie;
        try
        {
            infraMovie = await context.Movies.Include(m => m.Genres)
                                             .Where(m => m.Id == domainMovie.Id)
                                             .SingleAsync();
        }
        catch (InvalidOperationException ex)
        {
            throw new MovieRepositoryException($"No movie with the ID {domainMovie.Id} was found!", ex);
        }

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

    public async Task DeleteMoviesAsync(IEnumerable<Guid> movieIdsToDelete)
    {
        // Deleting this way and not via the ExecuteDeleteAsync()
        // as that solution would leave references to the deleted Movie
        // entitites in the Genre navigation properties.
        // This solution is not as fast as we need to make a DB round trip.
        var movies = await context.Movies.Where(m => movieIdsToDelete.Contains(m.Id))
                                         .ToListAsync();

        if (movies.Count != movieIdsToDelete.Count())
        {
            var foundIds = movies.Select(m => m.Id);
            var missingIds = movieIdsToDelete.Except(foundIds).ToList();
            var missingIdsAsString = string.Join(", ", missingIds);
            throw new MovieRepositoryException($"Not every requested ID exists! Missing IDs: {missingIdsAsString}");
        }

        context.RemoveRange(movies);
        await context.SaveChangesAsync();
    }
}
