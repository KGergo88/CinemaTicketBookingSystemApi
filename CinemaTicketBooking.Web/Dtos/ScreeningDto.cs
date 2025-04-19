using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos;

public class ScreeningDto
{
    [Required]
    public Guid AuditoriumId { get; set; }

    [Required]
    public Guid MovieId { get; set; }

    [Required]
    public DateTimeOffset ShowTime { get; set; }

    [Required]
    public string Language { get; set; }

    public string? Subtitles { get; set; }
}
