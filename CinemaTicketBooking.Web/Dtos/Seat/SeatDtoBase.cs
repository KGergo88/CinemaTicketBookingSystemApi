using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos.Seat;

public abstract class SeatDtoBase
{
    [Required]
    [Range(1, int.MaxValue)]
    public int Row { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Column { get; set; }
}
