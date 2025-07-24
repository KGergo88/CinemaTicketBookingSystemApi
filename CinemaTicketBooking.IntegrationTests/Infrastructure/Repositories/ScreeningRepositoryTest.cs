using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories.Exceptions;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.DatabaseSeeding;
using CinemaTicketBooking.Infrastructure.Entities;
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
            var exception = await Record.ExceptionAsync(async () => await screeningRepository.FindScreeningIdsInTimeFrameAsync(auditoriumId,
                                                                                                                               timeFrameStart,
                                                                                                                               timeFrameDuration));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<RepositoryException>(exception);
            Assert.Equal(exception.Message, $"{nameof(timeFrameDuration)} shall be greater than zero! Actual value: {timeFrameDuration}");
        }

        public static IEnumerable<object[]> FindScreeningIdsInTimeFrameAsyncTimeFrameOverlapCasesData()
        {
            // Screening ends before timeframe
            {
                var seedData = new DefaultSeedData();
                var auditoriumId = seedData.Auditoriums.First().Id;
                var movie = seedData.Movies.First();

                var timeFrameStart = DateTimeOffset.Now;
                var timeFrameDuration = TimeSpan.FromHours(2);

                var screening = new ScreeningEntity
                {
                    Id = Guid.NewGuid(),
                    AuditoriumId = auditoriumId,
                    MovieId = movie.Id,
                    Showtime = timeFrameStart - TimeSpan.FromSeconds(movie.DurationInSeconds),
                    Language = "English",
                };
                seedData.Screenings.Add(screening);

                var expectedFoundScreeningIds = new List<Guid>();

                yield return
                [
                    seedData,
                    auditoriumId,
                    timeFrameStart,
                    timeFrameDuration,
                    expectedFoundScreeningIds
                ];
            }

            // Screening ends one microsecond in the timeframe
            {
                var seedData = new DefaultSeedData();
                var auditoriumId = seedData.Auditoriums.First().Id;
                var movie = seedData.Movies.First();

                var timeFrameStart = DateTimeOffset.Now;
                var timeFrameDuration = TimeSpan.FromHours(2);

                var screening = new ScreeningEntity
                {
                    Id = Guid.NewGuid(),
                    AuditoriumId = auditoriumId,
                    MovieId = movie.Id,
                    Showtime = timeFrameStart - TimeSpan.FromSeconds(movie.DurationInSeconds) + TimeSpan.FromMicroseconds(1),
                    Language = "English",
                };
                seedData.Screenings.Add(screening);

                var expectedFoundScreeningIds = new List<Guid>()
                {
                    screening.Id
                };

                yield return
                [
                    seedData,
                    auditoriumId,
                    timeFrameStart,
                    timeFrameDuration,
                    expectedFoundScreeningIds
                ];
            }

            // Screening starts and ends within the timeframe
            {
                var seedData = new DefaultSeedData();
                var auditoriumId = seedData.Auditoriums.First().Id;
                var movie = seedData.Movies.First();

                var timeFrameStart = DateTimeOffset.Now;
                var timeFrameDuration = TimeSpan.FromSeconds(movie.DurationInSeconds) + TimeSpan.FromMinutes(15);

                var screening = new ScreeningEntity
                {
                    Id = Guid.NewGuid(),
                    AuditoriumId = auditoriumId,
                    MovieId = movie.Id,
                    Showtime = timeFrameStart + TimeSpan.FromMinutes(15),
                    Language = "English",
                };
                seedData.Screenings.Add(screening);

                var expectedFoundScreeningIds = new List<Guid>()
                {
                    screening.Id
                };

                yield return
                [
                    seedData,
                    auditoriumId,
                    timeFrameStart,
                    timeFrameDuration,
                    expectedFoundScreeningIds
                ];
            }

            // Screening starts one microsecond before the end of the timeframe
            {
                var seedData = new DefaultSeedData();
                var auditoriumId = seedData.Auditoriums.First().Id;
                var movie = seedData.Movies.First();

                var timeFrameStart = DateTimeOffset.Now;
                var timeFrameDuration = TimeSpan.FromHours(2);

                var screening = new ScreeningEntity
                {
                    Id = Guid.NewGuid(),
                    AuditoriumId = auditoriumId,
                    MovieId = movie.Id,
                    Showtime = timeFrameStart + timeFrameDuration - TimeSpan.FromMicroseconds(1),
                    Language = "English",
                };
                seedData.Screenings.Add(screening);

                var expectedFoundScreeningIds = new List<Guid>()
                {
                    screening.Id
                };

                yield return
                [
                    seedData,
                    auditoriumId,
                    timeFrameStart,
                    timeFrameDuration,
                    expectedFoundScreeningIds
                ];
            }

            // Screening completely overlaps the timeframe
            {
                var seedData = new DefaultSeedData();
                var auditoriumId = seedData.Auditoriums.First().Id;
                var movie = seedData.Movies.First();

                var timeFrameStart = DateTimeOffset.Now;
                var timeFrameDuration = TimeSpan.FromHours(2);

                var screening = new ScreeningEntity
                {
                    Id = Guid.NewGuid(),
                    AuditoriumId = auditoriumId,
                    MovieId = movie.Id,
                    Showtime = timeFrameStart - TimeSpan.FromHours(1),
                    Language = "English",
                };
                seedData.Screenings.Add(screening);

                var timeFrameEnd = timeFrameStart + timeFrameDuration;
                var timeBetweenShowtimeAndTimeFrameEnd = timeFrameEnd - screening.Showtime;
                var movieDuration = timeBetweenShowtimeAndTimeFrameEnd + TimeSpan.FromHours(1);
                movie.DurationInSeconds = (int)movieDuration.TotalSeconds;

                var expectedFoundScreeningIds = new List<Guid>()
                {
                    screening.Id
                };

                yield return
                [
                    seedData,
                    auditoriumId,
                    timeFrameStart,
                    timeFrameDuration,
                    expectedFoundScreeningIds
                ];
            }

            // Screening starts after the timeframe
            {
                var seedData = new DefaultSeedData();
                var auditoriumId = seedData.Auditoriums.First().Id;
                var movie = seedData.Movies.First();

                var timeFrameStart = DateTimeOffset.Now;
                var timeFrameDuration = TimeSpan.FromHours(2);

                var screening = new ScreeningEntity
                {
                    Id = Guid.NewGuid(),
                    AuditoriumId = auditoriumId,
                    MovieId = movie.Id,
                    Showtime = timeFrameStart + timeFrameDuration,
                    Language = "English",
                };
                seedData.Screenings.Add(screening);

                var expectedFoundScreeningIds = new List<Guid>();

                yield return
                [
                    seedData,
                    auditoriumId,
                    timeFrameStart,
                    timeFrameDuration,
                    expectedFoundScreeningIds
                ];
            }

            // Multiple screenings in timeframe
            {
                var seedData = new DefaultSeedData();
                var auditoriumId = seedData.Auditoriums.First().Id;
                var movie = seedData.Movies.First();

                var timeFrameStart = DateTimeOffset.Now;
                var timeFrameDuration = TimeSpan.FromHours(2);

                var firstScreening = new ScreeningEntity
                {
                    Id = Guid.NewGuid(),
                    AuditoriumId = auditoriumId,
                    MovieId = movie.Id,
                    Showtime = timeFrameStart,
                    Language = "English",
                };

                var secondScreening = new ScreeningEntity
                {
                    Id = Guid.NewGuid(),
                    AuditoriumId = auditoriumId,
                    MovieId = movie.Id,
                    Showtime = timeFrameStart + TimeSpan.FromMinutes(1),
                    Language = "English",
                };

                seedData.Screenings.AddRange([firstScreening, secondScreening]);

                var expectedFoundScreeningIds = new List<Guid>()
                {
                    firstScreening.Id,
                    secondScreening.Id
                };

                yield return
                [
                    seedData,
                    auditoriumId,
                    timeFrameStart,
                    timeFrameDuration,
                    expectedFoundScreeningIds
                ];
            }

            // Multiple screenings of which one is in timeframe
            {
                var seedData = new DefaultSeedData();
                var auditoriumId = seedData.Auditoriums.First().Id;
                var movie = seedData.Movies.First();

                var timeFrameStart = DateTimeOffset.Now;
                var timeFrameDuration = TimeSpan.FromHours(2);

                var firstScreening = new ScreeningEntity
                {
                    Id = Guid.NewGuid(),
                    AuditoriumId = auditoriumId,
                    MovieId = movie.Id,
                    Showtime = timeFrameStart  - TimeSpan.FromMinutes(movie.DurationInSeconds),
                    Language = "English",
                };

                var secondScreening = new ScreeningEntity
                {
                    Id = Guid.NewGuid(),
                    AuditoriumId = auditoriumId,
                    MovieId = movie.Id,
                    Showtime = timeFrameStart,
                    Language = "English",
                };

                seedData.Screenings.AddRange([firstScreening, secondScreening]);

                var expectedFoundScreeningIds = new List<Guid>()
                {
                    secondScreening.Id
                };

                yield return
                [
                    seedData,
                    auditoriumId,
                    timeFrameStart,
                    timeFrameDuration,
                    expectedFoundScreeningIds
                ];
            }
        }

        [Theory]
        [MemberData(nameof(FindScreeningIdsInTimeFrameAsyncTimeFrameOverlapCasesData))]
        async Task FindScreeningIdsInTimeFrameAsyncTimeFrameOverlapCases(SeedData seedData,
                                                                         Guid auditoriumId,
                                                                         DateTimeOffset timeFrameStart,
                                                                         TimeSpan timeFrameDuration,
                                                                         List<Guid> expectedFoundScreeningIds)
        {
            // Arrange
            await using var db = await CreateDatabaseAsync(seedData);
            var screeningRepository = new ScreeningRepository(mapper, db.Context);

            // Act
            var foundScreeningIds = await screeningRepository.FindScreeningIdsInTimeFrameAsync(auditoriumId,
                                                                                               timeFrameStart,
                                                                                               timeFrameDuration);

            // Assert
            Assert.NotNull(foundScreeningIds);
            // Comparing the screening IDs without expecting that their order matches
            Assert.Equal(expectedFoundScreeningIds.Count, foundScreeningIds.Count);
            Assert.All(foundScreeningIds, foundScreeningId => Assert.Contains(foundScreeningId, expectedFoundScreeningIds));
        }

        #endregion
    }
}
