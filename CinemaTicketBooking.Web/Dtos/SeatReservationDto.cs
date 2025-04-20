using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos;

public class SeatReservationDto
{
    [Required]
    public required Guid SeatId { get; set; }

    [Required]
    public required PriceDto Price { get; set; }
}
