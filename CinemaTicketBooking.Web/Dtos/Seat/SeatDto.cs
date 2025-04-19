using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos.Seat;

public class SeatDto : SeatDtoBase
{
    [Required]
    public Guid Id { get; set; }
}
