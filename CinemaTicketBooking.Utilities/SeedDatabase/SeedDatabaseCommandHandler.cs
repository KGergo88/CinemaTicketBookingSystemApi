using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.DatabaseBindings;
using CinemaTicketBooking.Infrastructure.DatabaseSeeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CinemaTicketBooking.Utilities.SeedDatabase;

internal class SeedDatabaseCommandHandler
{
    private readonly ILogger logger;

    public SeedDatabaseCommandHandler(ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<SeedDatabaseCommandHandler>();
    }

    public async Task HandleAsync(string connectionString, string jsonFilePath)
    {
        logger.LogInformation($"Using JSON file at: {jsonFilePath}");

        var seedData = SeedDataLoader.LoadFromJson(jsonFilePath);
        if (seedData is null)
            throw new ApplicationException($"Could not load seed data from JSON file: {jsonFilePath}");

        var optionsBuilder = new DbContextOptionsBuilder<CinemaTicketBookingDbContext>();
        var databaseBinding = DatabaseBindingFactory.Create(connectionString);
        databaseBinding.SetDatabaseType(optionsBuilder, connectionString);

        using var context = new CinemaTicketBookingDbContext(optionsBuilder.Options);
        try
        {
            logger.LogInformation($"Opening database connection...");
            await context.Database.OpenConnectionAsync();

            logger.LogInformation($"Seeding the database...");
            context.AddRange(seedData);
            await context.SaveChangesAsync();

            logger.LogInformation($"Successfully seeded the database!");
        }
        catch
        {
            logger.LogCritical($"Failed to seed the database!");
            throw;
        }
        finally
        {
            logger.LogInformation($"Closing database connection...");
            await context.Database.CloseConnectionAsync();
        }
    }
}
