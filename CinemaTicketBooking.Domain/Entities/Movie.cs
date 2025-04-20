namespace CinemaTicketBooking.Domain.Entities;

public class Movie
{
    public Guid Id { get; set; }

    public required string Title { get; set; }

    public required int? ReleaseYear { get; set; }

    public string Description { get; set; } = "";

    public required TimeSpan Duration { get; set; }

    public required List<string> Genres { get; set; }
}
