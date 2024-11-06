using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos.AddTheater
{
    public class TheaterDto
    {
        Guid? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [MinLength(1)]
        public List<AuditoriumDto> Auditoriums { get; set; }
    }
}
