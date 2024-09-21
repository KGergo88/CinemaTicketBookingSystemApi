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

            // Assert
            Assert.Equivalent(expectedDomainEntities, movies, strict: true);
        }
    }
}
