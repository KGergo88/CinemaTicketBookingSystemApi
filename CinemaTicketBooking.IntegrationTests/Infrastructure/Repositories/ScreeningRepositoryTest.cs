﻿using AutoMapper;
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
        private readonly static SeedData seedData = new DefaultSeedData();

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

            yield return new object[] { domainScreenings };
        }

        [Theory]
        [MemberData(nameof(AddScreeningsAsyncCreatesScreeningsCorrectlyAsyncData))]
        async Task AddScreeningsAsyncCreatesScreeningsCorrectlyAsync(List<Screening> domainScreenings)
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
    }
}
