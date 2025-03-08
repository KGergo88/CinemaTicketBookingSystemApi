using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos;

public class TierDto
{
    [Required]
    public string Name { get; set; }

    [MinLength(1)]
    public List<SeatDto> Seats { get; set; }
}
