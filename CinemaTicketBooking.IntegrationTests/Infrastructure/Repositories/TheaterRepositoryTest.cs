using AutoMapper;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.DatabaseSeeding;
using CinemaTicketBooking.Infrastructure.Entities;
using CinemaTicketBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.IntegrationTests.Infrastructure.Repositories
{
    [Trait("Category", "LocalDbBasedTests")]
    public class TheaterRepositoryTest : TestDatabase
    {
        private readonly static IMapper mapper;
        private readonly static SeedData seedData = new DefaultSeedData();

        static TheaterRepositoryTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            mapper = mappingConfig.CreateMapper();
        }

        #region Helper methods

        async Task CheckStoredTheaterAsync(CinemaTicketBookingDbContext dbContext, Theater domainTheater)
        {
            var infraTheater = await dbContext.Theaters.Include(t => t.Auditoriums)
                                                       .ThenInclude(a => a.Tiers)
                                                       .ThenInclude(t => t.Seats)
                                                       .AsSingleQuery()
                                                       .SingleAsync(t => t.Name == domainTheater.Name
                                                                         && t.Address == domainTheater.Address);

            foreach (var domainAuditorium in domainTheater.Auditoriums)
            {
                CheckStoredAuditoriums(infraTheater.Auditoriums, domainAuditorium);
            }
        }

        void CheckStoredAuditoriums(ICollection<AuditoriumEntity> storedAuditoriums,
                                    Auditorium domainAuditorium)
        {
            var infraAuditorium = storedAuditoriums.Single(sa => sa.Name == domainAuditorium.Name);
            foreach(var domainTier in domainAuditorium.Tiers)
            {
                CheckStoredTiers(infraAuditorium.Tiers, domainTier);
            }
        }

        void CheckStoredTiers(ICollection<TierEntity> storedTiers,
                              Tier domainTier)
        {
            var infraTier = storedTiers.Single(st => st.Name == domainTier.Name);
            foreach (var domainSeat in domainTier.Seats)
            {
                CheckStoredSeat(infraTier.Seats, domainSeat);
            }
        }

        void CheckStoredSeat(ICollection<SeatEntity> storedSeats,
                             Seat domainSeat)
        {
            var infraSeat = storedSeats.Single(ss => ss.Row == domainSeat.Row
                                                     && ss.Column == domainSeat.Column);
        }

        #endregion

        #region AddTheatersAsync Tests

        public static IEnumerable<object[]> AddTheatersAsyncCreatesTheatersAndRelatedEntitiesCorrectlyAsyncData()
        {
            var domainTheaters = seedData.Theaters.Select(mapper.Map<Theater>)
                                                  .ToList();

            yield return new object[] { domainTheaters };
        }

        [Theory]
        [MemberData(nameof(AddTheatersAsyncCreatesTheatersAndRelatedEntitiesCorrectlyAsyncData))]
        async Task AddTheatersAsyncCreatesTheatersAndRelatedEntitiesCorrectlyAsync(List<Theater> domainTheaters)
        {
            // Arrange
            await using var db = await CreateDatabaseAsync();
            var dbContext = db.Context;
            var theaterRepository = new TheaterRepository(mapper, dbContext);

            // Act
            await theaterRepository.AddTheatersAsync(domainTheaters);

            // Assert
            foreach (var domainTheater in domainTheaters)
            {
                await CheckStoredTheaterAsync(dbContext, domainTheater);
            }
        }

        #endregion
    }
}
