using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos;

public class ScreeningDto
{
    [Required]
    public required Guid AuditoriumId { get; set; }

    [Required]
    public required Guid MovieId { get; set; }

    [Required]
    public required DateTimeOffset ShowTime { get; set; }

    [Required]
    public required string Language { get; set; }

    public string? Subtitles { get; set; }
}
