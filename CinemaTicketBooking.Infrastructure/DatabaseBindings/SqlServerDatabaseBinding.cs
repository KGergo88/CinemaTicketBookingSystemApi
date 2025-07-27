using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace CinemaTicketBooking.Infrastructure.DatabaseBindings;

public class SqlServerDatabaseBinding : IDatabaseBinding
{
    private static Regex IsUniqueIndexExceptionRegex { get; } = new(
        @"^Cannot insert duplicate key row in object '.+' with unique index '.+'\.",
        RegexOptions.Compiled);

    public void SetDatabaseType(DbContextOptionsBuilder optionsBuilder, string? connectionString)
    {
        optionsBuilder.UseSqlServer(connectionString);
    }

    public bool IsUniqueIndexException(DbUpdateException dbUpdateException)
    {
        if (dbUpdateException.InnerException is SqlException sqlException)
            return IsUniqueIndexExceptionRegex.IsMatch(sqlException.Message);

        return false;
    }
}
