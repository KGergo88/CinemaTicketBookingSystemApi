using AutoMapper;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.DatabaseBindings;
using CinemaTicketBooking.Infrastructure.DatabaseSeeding;
using CinemaTicketBooking.Infrastructure.Entities;
using CinemaTicketBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.IntegrationTests.Infrastructure.Repositories
{
    [Trait("Category", "LocalDbBasedTests")]
    public class MovieRepositoryWithSeedTest : TestDatabase
    {
        private readonly IMapper mapper;
        private readonly static SeedData seedData = new DefaultSeedData();
        private readonly IDatabaseBinding databaseBinding;

        public MovieRepositoryWithSeedTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            mapper = mappingConfig.CreateMapper();

            databaseBinding = DatabaseBindingFactory.Create("CinemaTicketBookingDbContext");
        }

        #region GetMoviesAsync Tests

        [Fact]
        async Task GetMoviesAsyncReturnsAllMoviesCorrectlyAsync()
        {
            // Arrange
            await using var db = await CreateDatabaseAsync(seedData);
            var movieRepository = new MovieRepository(mapper, databaseBinding, db.Context);

            var expectedDomainMovies = mapper.Map<List<Movie>>(seedData.Movies);

            // Act
            var domainMovies = await movieRepository.GetMoviesAsync();

            // Assert
            Assert.Equivalent(expectedDomainMovies, domainMovies, strict: true);
        }

        public static IEnumerable<object[]> UpdateMovieAsyncUpdatesMoviesAndGenresCorrectlyAsyncData()
        {
            foreach (var infraMovie in seedData.Movies)
            {
                yield return new object[]
                {
                    infraMovie
                };
            }
        }

        #endregion

        #region UpdateMovieAsync Tests

        [Theory]
        [MemberData(nameof(UpdateMovieAsyncUpdatesMoviesAndGenresCorrectlyAsyncData))]
        async Task UpdateMovieAsyncUpdatesMoviesAndGenresCorrectlyAsync(MovieEntity infraMovieToUpdate)
        {
            // Arrange
            await using var db = await CreateDatabaseAsync(seedData);
            var movieRepository = new MovieRepository(mapper, databaseBinding, db.Context);

            infraMovieToUpdate.Title = "Updated " + infraMovieToUpdate.Title;
            infraMovieToUpdate.ReleaseYear += 1;
            infraMovieToUpdate.Description = "Updated " + infraMovieToUpdate.Description;
            infraMovieToUpdate.DurationInSeconds += 1;
            infraMovieToUpdate.Genres.Add(new GenreEntity { Name = "Not existing test genre" });
            var expectedDomainMovie = mapper.Map<Movie>(infraMovieToUpdate);

            // Act
            await movieRepository.UpdateMovieAsync(expectedDomainMovie);

            // Assert
            var updatedMovie = await db.Context.Movies.Include(m => m.Genres)
                                                      .SingleAsync(m => m.Id == expectedDomainMovie.Id);

            Assert.Equal(expectedDomainMovie.Title, updatedMovie.Title);
            Assert.Equal(expectedDomainMovie.ReleaseYear, updatedMovie.ReleaseYear);
            Assert.Equal(expectedDomainMovie.Description, updatedMovie.Description);
            Assert.Equal(expectedDomainMovie.Duration.TotalSeconds, updatedMovie.DurationInSeconds);
            Assert.Equal(expectedDomainMovie.Genres.Count, updatedMovie.Genres.Count);
            foreach (var expectedGenre in expectedDomainMovie.Genres)
            {
                var storedGenre = updatedMovie.Genres.Where(g => g.Name == expectedGenre).Single();
            }
        }

        #endregion

        #region DeleteMoviesAsync Tests

        public static IEnumerable<object[]> DeleteMoviesAsyncDeletesMoviesCorrectlyAsyncData()
        {
            foreach (var infraMovie in seedData.Movies)
            {
                yield return new object[]
                {
                    new List<Guid>() { infraMovie.Id }
                };
            }

            yield return new object[]
            {
                seedData.Movies.Select(m => m.Id)
                               .ToList()
            };
        }

        [Theory]
        [MemberData(nameof(DeleteMoviesAsyncDeletesMoviesCorrectlyAsyncData))]
        async Task DeleteMoviesAsyncDeletesMoviesCorrectlyAsync(List<Guid> movieIdsToDelete)
        {
            // Arrange
            await using var db = await CreateDatabaseAsync(seedData);
            var movieRepository = new MovieRepository(mapper, databaseBinding, db.Context);

            // Act
            await movieRepository.DeleteMoviesAsync(movieIdsToDelete);

            // Assert
            var storedMovies = await db.Context.Movies.ToListAsync();
            var storedGenres = await db.Context.Genres.Include(g => g.Movies)
                                                      .ToListAsync();
            foreach (var deletedMovieGuid in movieIdsToDelete)
            {
                Assert.DoesNotContain(storedMovies, sm => sm.Id == deletedMovieGuid);
                Assert.DoesNotContain(storedGenres, sg => sg.Movies.Any(m => m.Id == deletedMovieGuid));
            }
        }

        #endregion

        #region AddMoviesAsync Tests

        public static IEnumerable<object[]> AddMoviesAsyncCreatesMoviesAndGenresCorrectlyAsyncData()
        {
            var sleepyHollowMovie = new Movie
            {
                Title = "Sleepy Hollow",
                ReleaseYear = 1999,
                Description = "A movie about a headless horseman chopping other people´s heads off",
                Duration = TimeSpan.FromSeconds(105),
                Genres = new List<string> { "Dark Fantasy", "Slasher Horror", "Supernatural Horror", "Fantasy", "Horror", "Mistery" }
            };
            var iAmLegendMovie = new Movie
            {
                Title = "I Am Legend",
                ReleaseYear = 2007,
                Description = "A movie about people turning into zombies after getting vaccinated",
                Duration = TimeSpan.FromSeconds(101),
                Genres = new List<string> { "Dystopian Sci-Fi", "Survival", "Zombie Horror", "Action", "Drama", "Horror", "Sci-Fi", "Thriller" }
            };

            var domainMovies = new List<Movie>()
            {
                sleepyHollowMovie
            };

            yield return new object[]
            {
                domainMovies
            };

            domainMovies = new List<Movie>()
            {
                sleepyHollowMovie,
                iAmLegendMovie
            };

            yield return new object[]
            {
                domainMovies
            };
        }

        [Theory]
        [MemberData(nameof(AddMoviesAsyncCreatesMoviesAndGenresCorrectlyAsyncData))]
        async Task AddMoviesAsyncCreatesMoviesAndGenresCorrectlyAsync(List<Movie> domainMovies)
        {
            // Arrange
            await using var db = await CreateDatabaseAsync();
            var movieRepository = new MovieRepository(mapper, databaseBinding, db.Context);

            // Act
            await movieRepository.AddMoviesAsync(domainMovies);

            // Assert
            var infraMovies = await db.Context.Movies.ToListAsync();
            var infraGenres = await db.Context.Genres.ToListAsync();

            Assert.Equal(domainMovies.Count, infraMovies.Count);
            var expectedKnownGenreNames = domainMovies.SelectMany(dm => dm.Genres)
                                                      .Distinct()
                                                      .ToList();
            Assert.Equal(expectedKnownGenreNames.Count, infraGenres.Count);
            foreach (var domainMovie in domainMovies)
            {
                var infraMovie = infraMovies.Single(im => im.Title == domainMovie.Title
                                                          && im.ReleaseYear == domainMovie.ReleaseYear);
                Assert.Equal(domainMovie.Description, infraMovie.Description);
                Assert.Equal(domainMovie.Title, infraMovie.Title);

                Assert.Equal(domainMovie.Genres.Count, infraMovie.Genres.Count);
                foreach (var domainGenre in domainMovie.Genres)
                {
                    var infraGenre = infraGenres.Single(ig => ig.Name == domainGenre);
                    Assert.Contains(infraMovie, infraGenre.Movies);
                }
            }
        }

        #endregion
    }
}
