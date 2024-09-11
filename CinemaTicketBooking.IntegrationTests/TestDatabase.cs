using CinemaTicketBooking.Infrastructure;
using System.Runtime.CompilerServices;

namespace CinemaTicketBooking.IntegrationTests
{
    public sealed class TestDatabase : IAsyncDisposable
    {
        private static readonly SqlInstance<CinemaTicketBookingDbContext> sqlInstance;
        private SqlDatabase<CinemaTicketBookingDbContext>? database;

        static TestDatabase()
        {
            sqlInstance ??= new(constructInstance: builder => new(builder.Options));
        }

        public async ValueTask DisposeAsync()
        {
            if (database is not null)
            {
                await database.Context.Database.EnsureDeletedAsync();
                await database.DisposeAsync();
            }
        }

        public async Task<CinemaTicketBookingDbContext> GetContextAsync(
            [CallerFilePath] string testFile = "",
            string? databaseSuffix = null,
            [CallerMemberName] string memberName = "")
        {
            // Generating a unique suffix for the database name if none was provided
            databaseSuffix ??= Guid.NewGuid().ToString();

            if (database is null)
            {
                database = await sqlInstance.Build(testFile, databaseSuffix, memberName);
                await database.Context.Database.EnsureCreatedAsync();
            }

            return database.Context;
        }
    }
}
