using System.Text.Json;

namespace CinemaTicketBooking.Infrastructure.DatabaseSeeding;

public class SeedDataLoader
{
    public static IEnumerable<object> LoadFromJson(string path)
    {
        if (!Path.Exists(path))
            throw new ArgumentException($"The path \"{path}\" does not exist!");

        var jsonData = File.ReadAllText(path);
        var seedData = JsonSerializer.Deserialize<SeedData>(jsonData);
        if (seedData is null)
            throw new ArgumentException($"Could not serialize JSON file: {path}");

        var seedDataEntities = seedData.ToObjects();

        return seedDataEntities;
    }
}
