using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.Infrastructure.DatabaseBindings;

public interface IDatabaseBinding
{
    public void SetDatabaseType(DbContextOptionsBuilder options, string? connectionString);
}
