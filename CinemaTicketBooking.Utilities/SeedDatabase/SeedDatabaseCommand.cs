using System.CommandLine;

namespace CinemaTicketBooking.Utilities.SeedDatabase;

internal class SeedDatabaseCommand : Command
{
    public Argument<string> ConnectionStringArgument { get; } = new Argument<string>("connection_string", "The database connection string");
    public Argument<string> JsonPathArgument { get; } = new Argument<string>("json_path", "The path to the JSON file");

    public SeedDatabaseCommand() : base("seed_database", "Seeds a database with data from a JSON file")
    {
        AddArgument(ConnectionStringArgument);
        AddArgument(JsonPathArgument);
    }
}