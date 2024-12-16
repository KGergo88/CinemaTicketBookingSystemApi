namespace CinemaTicketBooking.Infrastructure.DatabaseBindings;

public class DatabaseBindingFactory
{
    public static IDatabaseBinding Create(string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        // There are no other supported databases at the moment
        return new SqlServerDatabaseBinding();
    }
}