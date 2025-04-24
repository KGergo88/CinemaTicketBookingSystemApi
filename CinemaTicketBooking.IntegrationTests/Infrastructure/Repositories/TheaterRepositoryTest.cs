using AutoMapper;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.Entities;
using CinemaTicketBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.IntegrationTests.Infrastructure.Repositories
{
    [Trait("Category", "LocalDbBasedTests")]
    public class TheaterRepositoryTest : TestDatabase
    {
        private IMapper mapper;

        public TheaterRepositoryTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            mapper = mappingConfig.CreateMapper();
        }

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

        #region AddTheatersAsync Tests

        public static IEnumerable<object[]> AddTheatersAsyncCreatesTheatersAndRelatedEntitiesCorrectlyAsyncData()
        {
            yield return new object[] {
                new List<Theater>{
                    new Theater
                    {
                        Name = "Elit Mozi",
                        Address = "Sopron, Torna u. 14, 9400 Hungary",
                        Auditoriums = new List<Auditorium>
                        {
                            new Auditorium
                            {
                                Name = "Huszárik terem",
                                Tiers = new List<Tier>
                                {
                                    new Tier
                                    {
                                        Name = "Standard tier",
                                        Seats = new List<Seat>
                                        {
                                            new Seat { Row = 1, Column = 1 },
                                            new Seat { Row = 7, Column = 4 }
                                        }
                                    }
                                }
                            },
                            new Auditorium
                            {
                                Name = "Nagyterem",
                                Tiers = new List<Tier>
                                {
                                    new Tier
                                    {
                                        Name = "Nézötér",
                                        Seats = new List<Seat>
                                        {
                                            new Seat { Row = 1, Column = 1 },
                                            new Seat { Row = 13, Column = 8 }
                                        }
                                    },
                                    new Tier
                                    {
                                        Name = "Erkély",
                                        Seats = new List<Seat>
                                        {
                                            new Seat { Row = 1, Column = 1 },
                                            new Seat { Row = 4, Column = 10 }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(AddTheatersAsyncCreatesTheatersAndRelatedEntitiesCorrectlyAsyncData))]
        public async Task AddTheatersAsyncCreatesTheatersAndRelatedEntitiesCorrectlyAsync(List<Theater> domainTheaters)
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
