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
            var dbName = Guid.NewGuid().ToString();

            IEnumerable<object>? entities = null;
            if (!string.IsNullOrEmpty(seedDataJsonPath))
                entities = SeedDataLoader.LoadFromJson(seedDataJsonPath);

            return sqlInstance.Build(dbName, entities);
        }
    }
}
