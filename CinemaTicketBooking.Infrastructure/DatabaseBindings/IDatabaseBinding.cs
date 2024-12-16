using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.Infrastructure.DatabaseBindings;

public interface IDatabaseBinding
{
    public void SetDatabaseType(DbContextOptionsBuilder options, string? connectionString);

    public bool IsUniqueIndexException(DbUpdateException exception);
}
