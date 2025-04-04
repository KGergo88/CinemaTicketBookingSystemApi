namespace CinemaTicketBooking.Domain.Entities;

public class Screening
{
    public Guid? Id { get; set; }

    public required Guid AuditoriumId { get; set; }

    public required Guid MovieId { get; set; }

    public required DateTimeOffset Showtime { get; set; }

    public required string Language { get; set; }

    public string? Subtitles { get; set; }
}
