namespace CinemaTicketBooking.Infrastructure.DatabaseBindings;

public class DatabaseBindingFactory
{
    public static IDatabaseBinding Create(string? connectionString)
    {
        // There are no other supported databases at the moment
        return new SqlServerDatabaseBinding();
    }
}