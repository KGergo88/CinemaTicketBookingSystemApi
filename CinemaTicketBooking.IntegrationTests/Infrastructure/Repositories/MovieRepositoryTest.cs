using AutoMapper;
using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.Repositories;

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
            var dbEntities = new List<CinemaTicketBooking.Infrastructure.Entities.Movie>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Sleepy Hollow",
                    ReleaseYear = 1999,
                    Description = "A movie about a headless horseman chopping other people´s heads off",
                    DurationInSeconds = 105
                }
            };
            yield return new object[]
            {
                dbEntities,
                new List<CinemaTicketBooking.Domain.Entities.Movie>()
                {
                    new()
                    {
                        Id = dbEntities[0].Id,
                        Title = dbEntities[0].Title,
                        ReleaseYear = dbEntities[0].ReleaseYear,
                        Description = dbEntities[0].Description,
                        Duration = TimeSpan.FromSeconds(dbEntities[0].DurationInSeconds),
                        Genre = Domain.Genre.Unknown
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetMoviesAsyncReturnsAllMoviesCorrectlyAsyncData))]
        async Task GetMoviesAsyncReturnsAllMoviesCorrectlyAsync(List<CinemaTicketBooking.Infrastructure.Entities.Movie> dbEntities,
                                                                List<CinemaTicketBooking.Domain.Entities.Movie> expectedDomainEntities)
        {
            // Arrange
            await using var db = new TestDatabase();
            var dbContext = await db.GetContextAsync();
            var moviesRepository = new MovieRepository(mapper, dbContext);
            await dbContext.Movies.AddRangeAsync(dbEntities);
            await dbContext.SaveChangesAsync();

            // Act
            var movies = await moviesRepository.GetMoviesAsync();

            // Assert
            Assert.Equivalent(expectedDomainEntities, movies, strict: true);
        }
    }
}
