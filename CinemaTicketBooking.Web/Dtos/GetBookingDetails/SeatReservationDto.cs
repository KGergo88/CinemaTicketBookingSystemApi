using CinemaTicketBooking.Web.Dtos.Seat;
using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos.GetBookingDetails;

public class SeatReservationDto
{
    [Required]
    public Guid? Id { get; set; }

    [Required]
    public string TierName { get; set; }

    [Required]
    public SeatWithIdDto Seat { get; set; }

    [Required]
    public PriceDto Price { get; set; }
}
