using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories.Exceptions;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.DatabaseSeeding;
using CinemaTicketBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.IntegrationTests.Infrastructure.Repositories
{
    [Trait("Category", "LocalDbBasedTests")]
    public class ScreeningRepositoryTest : TestDatabase
    {
        private readonly IMapper mapper;

        public ScreeningRepositoryTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            mapper = mappingConfig.CreateMapper();
        }

        #region AddScreeningsAsync Tests

        public static IEnumerable<object[]> AddScreeningsAsyncCreatesScreeningsCorrectlyAsyncData()
        {
            var seedData = new DefaultSeedData();
            var domainScreenings = new List<Screening>();
            var auditorium = seedData.Auditoriums.First();
            var infraMovies = seedData.Movies.ToList();
            foreach (var movie in infraMovies)
            {
                var domainScreening = new Screening
                {
                    AuditoriumId = auditorium.Id,
                    MovieId = movie.Id,
                    Showtime = DateTimeOffset.Now,
                    Language = "English",
                    Subtitles = "English"
                };
                domainScreenings.Add(domainScreening);
            }

            yield return new object[] { seedData, domainScreenings };
        }

        [Theory]
        [MemberData(nameof(AddScreeningsAsyncCreatesScreeningsCorrectlyAsyncData))]
        async Task AddScreeningsAsyncCreatesScreeningsCorrectlyAsync(SeedData seedData, List<Screening> domainScreenings)
        {
            // Arrange
            await using var db = await CreateDatabaseAsync(seedData);
            var screeningRepository = new ScreeningRepository(mapper, db.Context);

            // Act
            await screeningRepository.AddScreeningsAsync(domainScreenings);

            // Assert
            var infraScreenings = await db.Context.Screenings.ToListAsync();
            Assert.Equal(domainScreenings.Count, infraScreenings.Count);
            foreach (var domainScreening in domainScreenings)
            {
                var infraScreening = infraScreenings.Single(s => s.Showtime == domainScreening.Showtime);
                Assert.Equal(domainScreening.AuditoriumId, infraScreening.AuditoriumId);
                Assert.Equal(domainScreening.MovieId, infraScreening.MovieId);
                Assert.Equal(domainScreening.Language, infraScreening.Language);
                Assert.Equal(domainScreening.Subtitles, infraScreening.Subtitles);
            }
        }

        #endregion

        #region FindScreeningIdsInTimeFrameAsync Tests

        public static TheoryData<DateTimeOffset, TimeSpan> FindScreeningIdsInTimeFrameAsyncThrowsForInvalidTimeFramesData()
        {
            var theoryData = new TheoryData<DateTimeOffset, TimeSpan>
            {
                { DateTimeOffset.Now, TimeSpan.Zero },
                { DateTimeOffset.UnixEpoch, TimeSpan.Zero - TimeSpan.FromSeconds(1)}
            };

            return theoryData;
        }

        [Theory]
        [MemberData(nameof(FindScreeningIdsInTimeFrameAsyncThrowsForInvalidTimeFramesData))]
        async Task FindScreeningIdsInTimeFrameAsyncThrowsForInvalidTimeFrames(DateTimeOffset timeFrameStart, TimeSpan timeFrameDuration)
        {
            // Arrange
            await using var db = await CreateDatabaseAsync();
            var screeningRepository = new ScreeningRepository(mapper, db.Context);
            var auditoriumId = Guid.NewGuid();

            // Act
            var exception = await Record.ExceptionAsync(async () => await screeningRepository.FindScreeningIdsInTimeFrameAsync(auditoriumId, timeFrameStart, timeFrameDuration));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<RepositoryException>(exception);
            Assert.Equal(exception.Message, $"{nameof(timeFrameDuration)} shall be greater than zero! Actual value: {timeFrameDuration}");
        }

        #endregion
    }
}
