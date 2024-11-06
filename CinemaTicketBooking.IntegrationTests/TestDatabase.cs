using CinemaTicketBooking.Infrastructure;
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
            [CallerMemberName] string memberName = "")
            {
                return sqlInstance.Build(dbName: Guid.NewGuid().ToString());
            }
    }
}
