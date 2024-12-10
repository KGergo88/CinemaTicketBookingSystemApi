using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos.AddScreening
{
    public class ScreeningDto
    {
        public Guid? Id { get; set; }

        [Required]
        public Guid AuditoriumId { get; set; }

        [Required]
        public Guid MovieId { get; set; }

        [Required]
        public DateTime ShowTime { get; set; }

        [Required]
        public string Language { get; set; }

        public string? Subtitles { get; set; }
    }
}
