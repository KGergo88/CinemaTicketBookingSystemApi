using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos;

public class SeatReservationDto
{
    [Required]
    public Guid SeatId { get; set; }

    [Required]
    public PriceDto Price { get; set; }
}
