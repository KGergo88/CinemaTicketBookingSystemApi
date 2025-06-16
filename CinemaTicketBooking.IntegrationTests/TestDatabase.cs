using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.DatabaseSeeding;

namespace CinemaTicketBooking.IntegrationTests
{
    public abstract class TestDatabase
    {
        private readonly static SqlInstance<CinemaTicketBookingDbContext> sqlInstance;

        static TestDatabase() =>
            sqlInstance = new(
                constructInstance: builder => new(builder.Options));

        internal static Task<SqlDatabase<CinemaTicketBookingDbContext>> CreateDatabaseAsync(
            string? seedDataJsonPath = null)
        {
            SeedData? seedData = null;
            if (!string.IsNullOrEmpty(seedDataJsonPath))
                seedData = SeedDataLoader.LoadFromJson(seedDataJsonPath);

            return CreateDatabaseAsync(seedData);
        }

        internal static Task<SqlDatabase<CinemaTicketBookingDbContext>> CreateDatabaseAsync(
            SeedData? seedData)
        {
            var dbName = Guid.NewGuid().ToString();

            IEnumerable<object>? seedEntities = null;
            if (seedData is not null)
                seedEntities = seedData.ToObjects();

            return sqlInstance.Build(dbName, seedEntities);
        }
    }
}
