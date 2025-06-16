using System.Text.Json;

namespace CinemaTicketBooking.Infrastructure.DatabaseSeeding;

internal class SeedDataLoader
{
    internal static SeedData LoadFromJson(string path)
    {
        if (!Path.Exists(path))
            throw new ArgumentException($"The path \"{path}\" does not exist!");

        var jsonData = File.ReadAllText(path);
        var seedData = JsonSerializer.Deserialize<SeedData>(jsonData);
        if (seedData is null)
            throw new ArgumentException($"Could not serialize JSON file: {path}");

        return seedData;
    }
}
