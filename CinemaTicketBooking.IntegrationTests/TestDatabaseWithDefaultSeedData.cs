using AutoMapper;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.IntegrationTests
{
    public abstract class TestDatabaseWithDefaultSeedData
    {
        public const string defaultSeedDataJsonPath = "IntegrationTestsDefaultSeedData.json";

        public static Task<SqlDatabase<CinemaTicketBookingDbContext>> CreateDatabaseAsync()
        {
            return TestDatabase.CreateDatabaseAsync(defaultSeedDataJsonPath);
        }

        internal static async Task<TheaterEntity> GetTheaterEntityAsync(CinemaTicketBookingDbContext context, string name)
        {
            return await context.Theaters.Include(t => t.Auditoriums)
                                         .ThenInclude(a => a.Tiers)
                                         .ThenInclude(t => t.Seats)
                                         .Where(t => t.Name == name)
                                         .SingleAsync();
        }

        public static async Task<Theater> GetElitMoziTheaterAsync(CinemaTicketBookingDbContext context)
        {
            var entity = await GetTheaterEntityAsync(context, "Elit Mozi");
            var mapper = CreateMapper();
            return mapper.Map<Theater>(entity);
        }

        internal static async Task<AuditoriumEntity> GetAuditoriumEntityAsync(CinemaTicketBookingDbContext context, string name)
        {
            return await context.Auditoriums.Include(a => a.Tiers)
                                            .ThenInclude(t => t.Seats)
                                            .Where(a => a.Name == name)
                                            .SingleAsync();
        }

        public static async Task<Auditorium> GetHuszarikTeremAuditoriumAsync(CinemaTicketBookingDbContext context)
        {
            var entity = await GetAuditoriumEntityAsync(context, "Husz�rik terem");
            var mapper = CreateMapper();
            return mapper.Map<Auditorium>(entity);
        }

        internal static async Task<TierEntity> GetTierEntityAsync(CinemaTicketBookingDbContext context, string name)
        {
            return await context.Tiers.Include(t => t.Seats)
                                      .Where(a => a.Name == name)
                                      .SingleAsync();
        }

        internal static async Task<MovieEntity> GetMovieEntityAsync(CinemaTicketBookingDbContext context, string title)
        {
            return await context.Movies.Include(m => m.Genres)
                                       .Where(m => m.Title == title)
                                       .SingleAsync();
        }

        public static async Task<Movie> GetSleepyHollowMovieAsync(CinemaTicketBookingDbContext context)
        {
            var entity = await GetMovieEntityAsync(context, "Sleepy Hollow");
            var mapper = CreateMapper();
            return mapper.Map<Movie>(entity);
        }

        public static async Task<Movie> GetMelancholiaMovieAsync(CinemaTicketBookingDbContext context)
        {
            var entity = await GetMovieEntityAsync(context, "Melancholia");
            var mapper = CreateMapper();
            return mapper.Map<Movie>(entity);
        }

        internal static async Task<CustomerEntity> GetCustomerEntityAsync(CinemaTicketBookingDbContext context, string email)
        {
            return await context.Customers.Where(c => c.Email == email)
                                          .SingleAsync();
        }

        public static async Task<Customer> GetHansJuergenCustomerAsync(CinemaTicketBookingDbContext context)
        {
            var entity = await GetCustomerEntityAsync(context, "hans.juergen.waldmann@gmail.com");
            var mapper = CreateMapper();
            return mapper.Map<Customer>(entity);
        }

        public static async Task<Customer> GetHansSusieStressedCustomerAsync(CinemaTicketBookingDbContext context)
        {
            var entity = await GetCustomerEntityAsync(context, "susie.stressed@gmail.com");
            var mapper = CreateMapper();
            return mapper.Map<Customer>(entity);
        }

        private static IMapper CreateMapper()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            return mappingConfig.CreateMapper();
        }
    }
}
