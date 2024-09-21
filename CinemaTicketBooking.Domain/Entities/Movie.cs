namespace CinemaTicketBooking.Domain.Entities;

public class Movie
{
    // Id is required a there can be two movies with the same title (eg. remake)
    public required Guid Id { get; set; }

    public required string Title { get; set; }

    public required int? ReleaseYear { get; set; }

    public string Description { get; set; } = "";

    public required TimeSpan Duration { get; set; }

    public required List<string> Genres { get; set; }
}
