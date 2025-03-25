using AutoMapper;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.DatabaseBindings;
using CinemaTicketBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.IntegrationTests.Infrastructure.Repositories
{
    public class ScreeningRepositoryTest : TestDatabase
    {
        private IMapper mapper;
        private readonly IDatabaseBinding databaseBinding;

        public ScreeningRepositoryTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            mapper = mappingConfig.CreateMapper();

            databaseBinding = DatabaseBindingFactory.Create("CinemaTicketBookingDbContext");
        }

        #region AddScreeningsAsync Tests

        public static IEnumerable<object[]> AddScreeningsAsyncCreatesScreeningsCorrectlyAsyncData()
        {
            // The theaters and movies and all their child entities have their ids set as the AddScreening feature will be used
            // in a way that a screening can only be created for an existing theater and movie

            var sopronElitMoziHuszarikTerem = new Auditorium
            {
                Id = Guid.NewGuid(),
                Name = "Huszárik Terem",
                Tiers = new List<Tier>
                {
                    new Tier
                    {
                        Id = Guid.NewGuid(),
                        Name = "default",
                        Seats = new List<Seat>
                        {
                            new Seat
                            {
                                Id = Guid.NewGuid(),
                                Row = 1,
                                Column = 1
                            },
                            new Seat
                            {
                                Id = Guid.NewGuid(),
                                Row = 2,
                                Column = 1
                            },
                            new Seat
                            {
                                Id = Guid.NewGuid(),
                                Row = 3,
                                Column = 1
                            },
                            new Seat
                            {
                                Id = Guid.NewGuid(),
                                Row = 4,
                                Column = 1
                            }
                        }
                    }
                }
            };

            var theatersToSetup = new List<Theater>
            {
                new Theater
                {
                    Id = Guid.NewGuid(),
                    Name = "Elit Mozi",
                    Address = "Sopron, Torna u. 14, 9400 Hungary",
                    Auditoriums = new List<Auditorium>()
                    {
                        sopronElitMoziHuszarikTerem
                    }
                }
            };

            var sleepyHollowMovie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Sleepy Hollow",
                ReleaseYear = 1999,
                Description = "A movie about a headless horseman chopping other people´s heads off",
                Duration = TimeSpan.FromSeconds(105),
                Genres = new List<string> { "Dark Fantasy", "Slasher Horror", "Supernatural Horror", "Fantasy", "Horror", "Mistery" }
            };

            var iAmLegendMovie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "I Am Legend",
                ReleaseYear = 2007,
                Description = "A movie about people turning into zombies after getting vaccinated",
                Duration = TimeSpan.FromSeconds(101),
                Genres = new List<string> { "Dystopian Sci-Fi", "Survival", "Zombie Horror", "Action", "Drama", "Horror", "Sci-Fi", "Thriller" }
            };

            var moviesToSetup = new List<Movie>
            {
                sleepyHollowMovie,
                iAmLegendMovie
            };

            var sleepyHollowScreening = new Screening
            {
                Auditorium = sopronElitMoziHuszarikTerem,
                Movie = sleepyHollowMovie,
                Showtime = DateTimeOffset.Now,
                Language = "English",
                Subtitles = "English"
            };

            var iAmLegendScreening = new Screening
            {
                Auditorium = sopronElitMoziHuszarikTerem,
                Movie = iAmLegendMovie,
                Showtime = DateTimeOffset.Now,
                Language = "English",
                Subtitles = "Hungarian"
            };

            var domainScreenings = new List<Screening>()
            {
                sleepyHollowScreening,
                iAmLegendScreening
            };

            yield return new object[]
            {
                theatersToSetup,
                moviesToSetup,
                domainScreenings
            };
        }

        [Theory]
        [MemberData(nameof(AddScreeningsAsyncCreatesScreeningsCorrectlyAsyncData))]
        async Task AddScreeningsAsyncCreatesScreeningsCorrectlyAsync(List<Theater> theatersToSetup,
                                                                     List<Movie> moviesToSetup,
                                                                     List<Screening> domainScreenings)
        {
            // Arrange
            await using var db = await CreateDatabaseAsync();
            var dbContext = db.Context;
            var screeningsRepository = new ScreeningRepository(mapper, dbContext);
            {
                var theaterRepository = new TheaterRepository(mapper, dbContext);
                await theaterRepository.AddTheatersAsync(theatersToSetup);

                var movieRepository = new MovieRepository(mapper, databaseBinding, dbContext);
                await movieRepository.AddMoviesAsync(moviesToSetup);
            }

            // Act
            await screeningsRepository.AddScreeningsAsync(domainScreenings);

            // Assert
            var infraScreenings = await dbContext.Screenings.ToListAsync();
            Assert.Equal(domainScreenings.Count, infraScreenings.Count);
            foreach (var domainScreening in domainScreenings)
            {
                var infraScreening = infraScreenings.Single(s => s.Showtime == domainScreening.Showtime);
                Assert.Equal(domainScreening.Auditorium.Id, infraScreening.AuditoriumId);
                Assert.Equal(domainScreening.Movie.Id, infraScreening.MovieId);
                Assert.Equal(domainScreening.Language, infraScreening.Language);
                Assert.Equal(domainScreening.Subtitles, infraScreening.Subtitles);
            }
        }

        #endregion
    }
}
