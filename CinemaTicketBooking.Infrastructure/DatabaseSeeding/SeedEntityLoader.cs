namespace CinemaTicketBooking.Infrastructure.DatabaseSeeding;

public class SeedEntityLoader
{
    public static IEnumerable<object> LoadFromJson(string path)
    {
        var seedData = SeedDataLoader.LoadFromJson(path);
        return seedData.ToObjects();
    }
}
