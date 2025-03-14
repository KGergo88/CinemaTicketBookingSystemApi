using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos.Seat;

public class SeatWithIdDto : SeatWithoutIdDto
{
    [Required]
    public Guid Id { get; set; }
}
