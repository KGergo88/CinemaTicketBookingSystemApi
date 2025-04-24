﻿using AutoMapper;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.DatabaseBindings;
using CinemaTicketBooking.Infrastructure.Entities;
using CinemaTicketBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.IntegrationTests.Infrastructure.Repositories
{
    [Trait("Category", "LocalDbBasedTests")]
    public class MovieRepositoryTest : TestDatabase
    {
        private IMapper mapper;
        private readonly IDatabaseBinding databaseBinding;

        public MovieRepositoryTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            mapper = mappingConfig.CreateMapper();

            databaseBinding = DatabaseBindingFactory.Create("CinemaTicketBookingDbContext");
        }

        #region GetMoviesAsync Tests

        public static IEnumerable<object[]> GetMoviesAsyncReturnsAllMoviesCorrectlyAsyncData()
        {
            var darkFantasyGenre = new GenreEntity
            {
                Name = "Dark Fantasy",
                Movies = []
            };
            var slasherHorrorGenre = new GenreEntity
            {
                Name = "Slasher Horror",
                Movies = []
            };
            var supernaturalHorrorGenre = new GenreEntity
            {
                Name = "Supernatural Horror",
                Movies = []
            };
            var fantasyGenre = new GenreEntity
            {
                Name = "Fantasy",
                Movies = []
            };
            var horrorGenre = new GenreEntity
            {
                Name = "Horror",
                Movies = []
            };
            var misteryGenre = new GenreEntity
            {
                Name = "Mistery",
                Movies = []
            };
            var dystopianSciFiGenre = new GenreEntity
            {
                Name = "Dystopian Sci-Fi",
                Movies = []
            };
            var survivalGenre = new GenreEntity
            {
                Name = "Survival",
                Movies = []
            };
            var zombieHorrorGenre = new GenreEntity
            {
                Name = "Zombie Horror",
                Movies = []
            };
            var actionGenre = new GenreEntity
            {
                Name = "Action",
                Movies = []
            };
            var dramaGenre = new GenreEntity
            {
                Name = "Drama",
                Movies = []
            };
            var sciFiGenre = new GenreEntity
            {
                Name = "Sci-Fi",
                Movies = []
            };
            var thrillerGenre = new GenreEntity
            {
                Name = "Thriller",
                Movies = []
            };
            var sleepyHollowMovie = new MovieEntity
            {
                Id = Guid.NewGuid(),
                Title = "Sleepy Hollow",
                ReleaseYear = 1999,
                Description = "A movie about a headless horseman chopping other people´s heads off",
                DurationInSeconds = 105,
                Genres = new List<GenreEntity>
                {
                    darkFantasyGenre,
                    slasherHorrorGenre,
                    supernaturalHorrorGenre,
                    fantasyGenre,
                    horrorGenre,
                    misteryGenre
                }
            };
            var iAmLegendMovie = new MovieEntity
            {
                Id = Guid.NewGuid(),
                Title = "I Am Legend",
                ReleaseYear = 2007,
                Description = "A movie about people turning into zombies after getting vaccinated",
                DurationInSeconds = 101,
                Genres = new List<GenreEntity>
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

            var infraMovies = new List<MovieEntity>()
            {
                sleepyHollowMovie
            };
            var infraGenres = new List<GenreEntity>()
            {
                darkFantasyGenre,
                slasherHorrorGenre,
                supernaturalHorrorGenre,
                fantasyGenre,
                horrorGenre,
                misteryGenre
            };
            var expectedDomainMovies = new List<Movie>()
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

            infraMovies = new List<MovieEntity>()
            {
                sleepyHollowMovie,
                iAmLegendMovie
            };
            infraGenres = new List<GenreEntity>()
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
            expectedDomainMovies = new List<Movie>()
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
        async Task GetMoviesAsyncReturnsAllMoviesCorrectlyAsync(List<MovieEntity> infraMovies,
                                                                List<GenreEntity> infraGenres,
                                                                List<Movie> expectedDomainMovies)
        {
            // Arrange
            await using var db = await CreateDatabaseAsync();
            var dbContext = db.Context;
            var moviesRepository = new MovieRepository(mapper, databaseBinding, dbContext);
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
            var dbContext = db.Context;
            var moviesRepository = new MovieRepository(mapper, databaseBinding, dbContext);

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
            var darkFantasyGenre = new GenreEntity
            {
                Name = "Dark Fantasy",
                Movies = []
            };
            var slasherHorrorGenre = new GenreEntity
            {
                Name = "Slasher Horror",
                Movies = []
            };
            var supernaturalHorrorGenre = new GenreEntity
            {
                Name = "Supernatural Horror",
                Movies = []
            };
            var fantasyGenre = new GenreEntity
            {
                Name = "Fantasy",
                Movies = []
            };
            var horrorGenre = new GenreEntity
            {
                Name = "Horror",
                Movies = []
            };
            var misteryGenre = new GenreEntity
            {
                Name = "Mistery",
                Movies = []
            };
            var dystopianSciFiGenre = new GenreEntity
            {
                Name = "Dystopian Sci-Fi",
                Movies = []
            };
            var survivalGenre = new GenreEntity
            {
                Name = "Survival",
                Movies = []
            };
            var zombieHorrorGenre = new GenreEntity
            {
                Name = "Zombie Horror",
                Movies = []
            };
            var actionGenre = new GenreEntity
            {
                Name = "Action",
                Movies = []
            };
            var dramaGenre = new GenreEntity
            {
                Name = "Drama",
                Movies = []
            };
            var sciFiGenre = new GenreEntity
            {
                Name = "Sci-Fi",
                Movies = []
            };
            var thrillerGenre = new GenreEntity
            {
                Name = "Thriller",
                Movies = []
            };
            var infraGenres = new List<GenreEntity>()
            {
                darkFantasyGenre,
                slasherHorrorGenre,
                supernaturalHorrorGenre,
                fantasyGenre,
                horrorGenre,
                misteryGenre
            };
            var sleepyHollowMovie = new MovieEntity
            {
                Id = Guid.NewGuid(),
                Title = "Sleepy Hollow",
                ReleaseYear = 1999,
                Description = "A movie about a headless horseman chopping other people´s heads off",
                DurationInSeconds = 105,
                Genres = new List<GenreEntity>
                {
                    darkFantasyGenre,
                    slasherHorrorGenre,
                    supernaturalHorrorGenre,
                    fantasyGenre,
                    horrorGenre,
                    misteryGenre
                }
            };
            var iAmLegendMovie = new MovieEntity
            {
                Id = Guid.NewGuid(),
                Title = "I Am Legend",
                ReleaseYear = 2007,
                Description = "A movie about people turning into zombies after getting vaccinated",
                DurationInSeconds = 101,
                Genres = new List<GenreEntity>
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
            var infraMovies = new List<MovieEntity>
            {
                sleepyHollowMovie,
                iAmLegendMovie
            };

            yield return new object[]
            {
                infraMovies,
                infraGenres,
                new Movie
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
        async Task UpdateMovieAsyncUpdatesMoviesAndGenresCorrectlyAsync(List<MovieEntity> infraMovies,
                                                                        List<GenreEntity> infraGenres,
                                                                        Movie expectedDomainMovie)
        {
            // Arrange
            await using var db = await CreateDatabaseAsync();
            var dbContext = db.Context;
            var moviesRepository = new MovieRepository(mapper, databaseBinding, dbContext);
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

        #region DeleteMoviesAsync Tests

        public static IEnumerable<object[]> DeleteMoviesAsyncDeletesMoviesCorrectlyAsyncData()
        {
            var darkFantasyGenre = new GenreEntity
            {
                Name = "Dark Fantasy",
                Movies = []
            };
            var slasherHorrorGenre = new GenreEntity
            {
                Name = "Slasher Horror",
                Movies = []
            };
            var supernaturalHorrorGenre = new GenreEntity
            {
                Name = "Supernatural Horror",
                Movies = []
            };
            var fantasyGenre = new GenreEntity
            {
                Name = "Fantasy",
                Movies = []
            };
            var horrorGenre = new GenreEntity
            {
                Name = "Horror",
                Movies = []
            };
            var misteryGenre = new GenreEntity
            {
                Name = "Mistery",
                Movies = []
            };
            var dystopianSciFiGenre = new GenreEntity
            {
                Name = "Dystopian Sci-Fi",
                Movies = []
            };
            var survivalGenre = new GenreEntity
            {
                Name = "Survival",
                Movies = []
            };
            var zombieHorrorGenre = new GenreEntity
            {
                Name = "Zombie Horror",
                Movies = []
            };
            var actionGenre = new GenreEntity
            {
                Name = "Action",
                Movies = []
            };
            var dramaGenre = new GenreEntity
            {
                Name = "Drama",
                Movies = []
            };
            var sciFiGenre = new GenreEntity
            {
                Name = "Sci-Fi",
                Movies = []
            };
            var thrillerGenre = new GenreEntity
            {
                Name = "Thriller",
                Movies = []
            };
            var infraGenres = new List<GenreEntity>()
            {
                darkFantasyGenre,
                slasherHorrorGenre,
                supernaturalHorrorGenre,
                fantasyGenre,
                horrorGenre,
                misteryGenre
            };
            var sleepyHollowMovie = new MovieEntity
            {
                Id = Guid.NewGuid(),
                Title = "Sleepy Hollow",
                ReleaseYear = 1999,
                Description = "A movie about a headless horseman chopping other people´s heads off",
                DurationInSeconds = 105,
                Genres = new List<GenreEntity>
                {
                    darkFantasyGenre,
                    slasherHorrorGenre,
                    supernaturalHorrorGenre,
                    fantasyGenre,
                    horrorGenre,
                    misteryGenre
                }
            };
            var iAmLegendMovie = new MovieEntity
            {
                Id = Guid.NewGuid(),
                Title = "I Am Legend",
                ReleaseYear = 2007,
                Description = "A movie about people turning into zombies after getting vaccinated",
                DurationInSeconds = 101,
                Genres = new List<GenreEntity>
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
            var infraMovies = new List<MovieEntity>
            {
                sleepyHollowMovie,
                iAmLegendMovie
            };

            yield return new object[]
            {
                infraMovies,
                infraGenres,
                new List<Guid> { sleepyHollowMovie.Id }
            };
        }

        [Theory]
        [MemberData(nameof(DeleteMoviesAsyncDeletesMoviesCorrectlyAsyncData))]
        async Task DeleteMoviesAsyncDeletesMoviesCorrectlyAsync(List<MovieEntity> infraMovies,
                                                                List<GenreEntity> infraGenres,
                                                                List<Guid> movieIdsToDelete)
        {
            // Arrange
            await using var db = await CreateDatabaseAsync();
            var dbContext = db.Context;
            var moviesRepository = new MovieRepository(mapper, databaseBinding, dbContext);
            dbContext.Movies.AddRange(infraMovies);
            dbContext.Genres.AddRange(infraGenres);
            await dbContext.SaveChangesAsync();

            // Act
            await moviesRepository.DeleteMoviesAsync(movieIdsToDelete);

            // Assert
            var storedMovies = await dbContext.Movies.ToListAsync();
            var storedGenres = await dbContext.Genres.Include(g => g.Movies)
                                                     .ToListAsync();
            foreach (var deletedMovieGuid in movieIdsToDelete)
            {
                Assert.DoesNotContain(storedMovies, sm => sm.Id == deletedMovieGuid);
                Assert.DoesNotContain(storedGenres, sg => sg.Movies.Any(m => m.Id == deletedMovieGuid));
            }
        }

        #endregion
    }
}
