using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos.GetAvailableSeats;

public class SeatDto
{
    [Range(1, int.MaxValue)]
    public int Row { get; set; }

    [Range(1, int.MaxValue)]
    public int Column { get; set; }
}
