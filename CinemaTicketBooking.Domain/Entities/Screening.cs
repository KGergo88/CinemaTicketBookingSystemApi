namespace CinemaTicketBooking.Domain.Entities;

public class Screening
{
    public Guid? Id { get; set; }

    public required Auditorium Auditorium { get; set; }

    public required Movie Movie { get; set; }

    public required DateTimeOffset Showtime { get; set; }

    public required string Language { get; set; }

    public string? Subtitles { get; set; }
}
