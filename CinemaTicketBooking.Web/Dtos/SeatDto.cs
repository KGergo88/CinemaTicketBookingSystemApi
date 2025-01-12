using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos
{
    public class SeatDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Row { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Column { get; set; }
    }
}
