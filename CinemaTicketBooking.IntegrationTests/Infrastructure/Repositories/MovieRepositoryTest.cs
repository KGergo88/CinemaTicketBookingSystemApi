using AutoMapper;
using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.IntegrationTests.Infrastructure.Repositories
{
    public class MovieRepositoryTest
    {
        private IMapper mapper;

        public MovieRepositoryTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            mapper = mappingConfig.CreateMapper();
        }

        #region GetMoviesAsync Tests

        public static IEnumerable<object[]> GetMoviesAsyncReturnsAllMoviesCorrectlyAsyncData()
        {
            var darkFantasyGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Dark Fantasy"
            };
            var slasherHorrorGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Slasher Horror"
            };
            var supernaturalHorrorGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Supernatural Horror"
            };
            var fantasyGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Fantasy"
            };
            var horrorGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Horror"
            };
            var misteryGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Mistery"
            };
            var dystopianSciFiGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Dystopian Sci-Fi"
            };
            var survivalGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Survival"
            };
            var zombieHorrorGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Zombie Horror"
            };
            var actionGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Action"
            };
            var dramaGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Drama"
            };
            var sciFiGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Sci-Fi"
            };
            var thrillerGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Thriller"
            };
            var sleepyHollowMovie = new CinemaTicketBooking.Infrastructure.Entities.Movie
            {
                Id = Guid.NewGuid(),
                Title = "Sleepy Hollow",
                ReleaseYear = 1999,
                Description = "A movie about a headless horseman chopping other people´s heads off",
                DurationInSeconds = 105,
                Genres = new List<CinemaTicketBooking.Infrastructure.Entities.Genre>
                {
                    darkFantasyGenre,
                    slasherHorrorGenre,
                    supernaturalHorrorGenre,
                    fantasyGenre,
                    horrorGenre,
                    misteryGenre
                }
            };
            var iAmLegendMovie = new CinemaTicketBooking.Infrastructure.Entities.Movie
            {
                Id = Guid.NewGuid(),
                Title = "I Am Legend",
                ReleaseYear = 2007,
                Description = "A movie about people turning into zombies after getting vaccinated",
                DurationInSeconds = 101,
                Genres = new List<CinemaTicketBooking.Infrastructure.Entities.Genre>
                {
                    dystopianSciFiGenre,
                    survivalGenre,
                    zombieHorrorGenre,
                    actionGenre,
                    dramaGenre,
                    horrorGenre,
                    sciFiGenre,
                    thrillerGenre
                }
            };

            var infraMovies = new List<CinemaTicketBooking.Infrastructure.Entities.Movie>()
            {
                sleepyHollowMovie
            };
            var infraGenres = new List<CinemaTicketBooking.Infrastructure.Entities.Genre>()
            {
                darkFantasyGenre,
                slasherHorrorGenre,
                supernaturalHorrorGenre,
                fantasyGenre,
                horrorGenre,
                misteryGenre
            };
            var expectedDomainMovies = new List<CinemaTicketBooking.Domain.Entities.Movie>()
            {
                new()
                {
                    Id = sleepyHollowMovie.Id,
                    Title = sleepyHollowMovie.Title,
                    ReleaseYear = sleepyHollowMovie.ReleaseYear,
                    Description = sleepyHollowMovie.Description,
                    Duration = TimeSpan.FromSeconds(sleepyHollowMovie.DurationInSeconds),
                    Genres = new List<string> { "Dark Fantasy", "Slasher Horror", "Supernatural Horror", "Fantasy", "Horror", "Mistery" }
                }
            };

            yield return new object[]
            {
                infraMovies,
                infraGenres,
                expectedDomainMovies
            };

            infraMovies = new List<CinemaTicketBooking.Infrastructure.Entities.Movie>()
            {
                sleepyHollowMovie,
                iAmLegendMovie
            };
            infraGenres = new List<CinemaTicketBooking.Infrastructure.Entities.Genre>()
            {
                darkFantasyGenre,
                slasherHorrorGenre,
                supernaturalHorrorGenre,
                fantasyGenre,
                horrorGenre,
                misteryGenre,
                dystopianSciFiGenre,
                survivalGenre,
                zombieHorrorGenre,
                actionGenre,
                dramaGenre,
                sciFiGenre,
                thrillerGenre
            };
            expectedDomainMovies = new List<CinemaTicketBooking.Domain.Entities.Movie>()
            {
                new()
                {
                    Id = sleepyHollowMovie.Id,
                    Title = sleepyHollowMovie.Title,
                    ReleaseYear = sleepyHollowMovie.ReleaseYear,
                    Description = sleepyHollowMovie.Description,
                    Duration = TimeSpan.FromSeconds(sleepyHollowMovie.DurationInSeconds),
                    Genres = new List<string> { "Dark Fantasy", "Slasher Horror", "Supernatural Horror", "Fantasy", "Horror", "Mistery" }
                },
                new()
                {
                    Id = iAmLegendMovie.Id,
                    Title = iAmLegendMovie.Title,
                    ReleaseYear = iAmLegendMovie.ReleaseYear,
                    Description = iAmLegendMovie.Description,
                    Duration = TimeSpan.FromSeconds(iAmLegendMovie.DurationInSeconds),
                    Genres = new List<string> { "Dystopian Sci-Fi", "Survival", "Zombie Horror", "Action", "Drama", "Horror", "Sci-Fi", "Thriller" }
                }
            };

            yield return new object[]
            {
                infraMovies,
                infraGenres,
                expectedDomainMovies
            };
        }

        [Theory]
        [MemberData(nameof(GetMoviesAsyncReturnsAllMoviesCorrectlyAsyncData))]
        async Task GetMoviesAsyncReturnsAllMoviesCorrectlyAsync(List<CinemaTicketBooking.Infrastructure.Entities.Movie> infraMovies,
                                                                List<CinemaTicketBooking.Infrastructure.Entities.Genre> infraGenres,
                                                                List<CinemaTicketBooking.Domain.Entities.Movie> expectedDomainMovies)
        {
            // Arrange
            await using var db = new TestDatabase();
            var dbContext = await db.GetContextAsync();
            var moviesRepository = new MovieRepository(mapper, dbContext);
            dbContext.Genres.AddRange(infraGenres);
            dbContext.Movies.AddRange(infraMovies);
            await dbContext.SaveChangesAsync();

            // Act
            var domainMovies = await moviesRepository.GetMoviesAsync();

            // Assert
            Assert.Equivalent(expectedDomainMovies, domainMovies, strict: true);
        }

        #endregion

        #region AddMoviesAsync Tests

        public static IEnumerable<object[]> AddMoviesAsyncCreatesMoviesAndGenresCorrectlyAsyncData()
        {
            var sleepyHollowMovie = new CinemaTicketBooking.Domain.Entities.Movie
            {
                Title = "Sleepy Hollow",
                ReleaseYear = 1999,
                Description = "A movie about a headless horseman chopping other people´s heads off",
                Duration = TimeSpan.FromSeconds(105),
                Genres = new List<string> { "Dark Fantasy", "Slasher Horror", "Supernatural Horror", "Fantasy", "Horror", "Mistery" }
            };
            var iAmLegendMovie = new CinemaTicketBooking.Domain.Entities.Movie
            {
                Title = "I Am Legend",
                ReleaseYear = 2007,
                Description = "A movie about people turning into zombies after getting vaccinated",
                Duration = TimeSpan.FromSeconds(101),
                Genres = new List<string> { "Dystopian Sci-Fi", "Survival", "Zombie Horror", "Action", "Drama", "Horror", "Sci-Fi", "Thriller" }
            };

            var domainMovies = new List<CinemaTicketBooking.Domain.Entities.Movie>()
            {
                sleepyHollowMovie
            };

            yield return new object[]
            {
                domainMovies
            };

            domainMovies = new List<CinemaTicketBooking.Domain.Entities.Movie>()
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
        async Task AddMoviesAsyncCreatesMoviesAndGenresCorrectlyAsync(List<CinemaTicketBooking.Domain.Entities.Movie> domainMovies)
        {
            // Arrange
            await using var db = new TestDatabase();
            var dbContext = await db.GetContextAsync();
            var moviesRepository = new MovieRepository(mapper, dbContext);

            // Act
            await moviesRepository.AddMoviesAsync(domainMovies);

            // Assert
            var infraMovies = await dbContext.Movies.ToListAsync();
            var infraGenres = await dbContext.Genres.ToListAsync();

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

        #region UpdateMovieAsync Tests

        public static IEnumerable<object[]> UpdateMovieAsyncUpdatesMoviesAndGenresCorrectlyAsyncData()
        {
            var darkFantasyGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Dark Fantasy"
            };
            var slasherHorrorGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Slasher Horror"
            };
            var supernaturalHorrorGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Supernatural Horror"
            };
            var fantasyGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Fantasy"
            };
            var horrorGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Horror"
            };
            var misteryGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Mistery"
            };
            var dystopianSciFiGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Dystopian Sci-Fi"
            };
            var survivalGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Survival"
            };
            var zombieHorrorGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Zombie Horror"
            };
            var actionGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Action"
            };
            var dramaGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Drama"
            };
            var sciFiGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Sci-Fi"
            };
            var thrillerGenre = new CinemaTicketBooking.Infrastructure.Entities.Genre
            {
                Name = "Thriller"
            };
            var infraGenres = new List<CinemaTicketBooking.Infrastructure.Entities.Genre>()
            {
                darkFantasyGenre,
                slasherHorrorGenre,
                supernaturalHorrorGenre,
                fantasyGenre,
                horrorGenre,
                misteryGenre
            };
            var sleepyHollowMovie = new CinemaTicketBooking.Infrastructure.Entities.Movie
            {
                Id = Guid.NewGuid(),
                Title = "Sleepy Hollow",
                ReleaseYear = 1999,
                Description = "A movie about a headless horseman chopping other people´s heads off",
                DurationInSeconds = 105,
                Genres = new List<CinemaTicketBooking.Infrastructure.Entities.Genre>
                {
                    darkFantasyGenre,
                    slasherHorrorGenre,
                    supernaturalHorrorGenre,
                    fantasyGenre,
                    horrorGenre,
                    misteryGenre
                }
            };
            var iAmLegendMovie = new CinemaTicketBooking.Infrastructure.Entities.Movie
            {
                Id = Guid.NewGuid(),
                Title = "I Am Legend",
                ReleaseYear = 2007,
                Description = "A movie about people turning into zombies after getting vaccinated",
                DurationInSeconds = 101,
                Genres = new List<CinemaTicketBooking.Infrastructure.Entities.Genre>
                {
                    dystopianSciFiGenre,
                    survivalGenre,
                    zombieHorrorGenre,
                    actionGenre,
                    dramaGenre,
                    horrorGenre,
                    sciFiGenre,
                    thrillerGenre
                }
            };
            var infraMovies = new List<CinemaTicketBooking.Infrastructure.Entities.Movie>
            {
                sleepyHollowMovie,
                iAmLegendMovie
            };

            yield return new object[]
            {
                infraMovies,
                infraGenres,
                new CinemaTicketBooking.Domain.Entities.Movie
                {
                    Id = sleepyHollowMovie.Id,
                    Title = "Updated title",
                    ReleaseYear = 1234,
                    Description = "Updated description",
                    Duration = TimeSpan.FromSeconds(123),
                    Genres = new List<string>
                    {
                        "Updated genre"
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(UpdateMovieAsyncUpdatesMoviesAndGenresCorrectlyAsyncData))]
        async Task UpdateMovieAsyncUpdatesMoviesAndGenresCorrectlyAsync(List<CinemaTicketBooking.Infrastructure.Entities.Movie> infraMovies,
                                                                        List<CinemaTicketBooking.Infrastructure.Entities.Genre> infraGenres,
                                                                        CinemaTicketBooking.Domain.Entities.Movie expectedDomainMovie)
        {
            // Arrange
            await using var db = new TestDatabase();
            var dbContext = await db.GetContextAsync();
            var moviesRepository = new MovieRepository(mapper, dbContext);
            dbContext.Movies.AddRange(infraMovies);
            dbContext.Genres.AddRange(infraGenres);
            await dbContext.SaveChangesAsync();

            // Act
            await moviesRepository.UpdateMovieAsync(expectedDomainMovie);

            // Assert
            var updatedMovie = await dbContext.Movies.Include(m => m.Genres)
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
    }
}
