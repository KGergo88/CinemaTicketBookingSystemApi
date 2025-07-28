using CinemaTicketBooking.Infrastructure.DatabaseBindings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CinemaTicketBooking.Infrastructure;

public class CinemaTicketBookingDesignTimeDbContextFactory : IDesignTimeDbContextFactory<CinemaTicketBookingDbContext>
{
    public CinemaTicketBookingDbContext CreateDbContext(string[] args)
    {
        // There is no need for a connection string during design time.
        // For the .NET EF CLI's update command, the argument --connection can be used:
        // https://learn.microsoft.com/en-us/ef/core/cli/dotnet#dotnet-ef-database-update
        string? connectionString = null;

        var databaseBinding = DatabaseBindingFactory.Create(connectionString);
        var optionsBuilder = new DbContextOptionsBuilder<CinemaTicketBookingDbContext>();
        databaseBinding.SetDatabaseType(optionsBuilder, connectionString);

        return new CinemaTicketBookingDbContext(optionsBuilder.Options);
    }
}
