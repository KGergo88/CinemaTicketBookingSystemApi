using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.IntegrationTests
{
    [Trait("Category", "LocalDbBasedTests")]
    public class CinemaTicketBookingDbContextTest : TestDatabase
    {
        [Fact]
        async Task ThereAreNoPendingModelChanges()
        {
            // Arrange
            await using var db = await CreateDatabaseAsync();

            // Act
            var hasPendingModelChanges = db.Context.Database.HasPendingModelChanges();

            // Assert
            Assert.False(hasPendingModelChanges);
        }
    }
}
