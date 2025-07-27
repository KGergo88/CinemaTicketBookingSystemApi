using CinemaTicketBooking.Infrastructure.DatabaseBindings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CinemaTicketBooking.Infrastructure;

public class CinemaTicketBookingDesignTimeDbContextFactory : IDesignTimeDbContextFactory<CinemaTicketBookingDbContext>
{
    public CinemaTicketBookingDbContext CreateDbContext(string[] args)
    {
        var connectionString = "";

        var databaseBinding = DatabaseBindingFactory.Create(connectionString);
        var optionsBuilder = new DbContextOptionsBuilder<CinemaTicketBookingDbContext>();
        databaseBinding.SetDatabaseType(optionsBuilder, connectionString);

        return new CinemaTicketBookingDbContext(optionsBuilder.Options);
    }
}
