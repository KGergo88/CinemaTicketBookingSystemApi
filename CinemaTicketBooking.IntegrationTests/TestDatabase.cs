using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.DatabaseSeeding;
using System.Runtime.CompilerServices;

namespace CinemaTicketBooking.IntegrationTests
{
    public abstract class TestDatabase
    {
        static SqlInstance<CinemaTicketBookingDbContext> sqlInstance;

        static TestDatabase() =>
            sqlInstance = new(
                constructInstance: builder => new(builder.Options));

        public static Task<SqlDatabase<CinemaTicketBookingDbContext>> CreateDatabaseAsync(
            [CallerFilePath] string testFile = "",
            string? databaseSuffix = null,
            [CallerMemberName] string memberName = "",
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
