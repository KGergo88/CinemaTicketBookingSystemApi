using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos;

public class PricingDto
{
    [Required]
    public required Guid ScreeningId { get; set; }

    [Required]
    public required Guid TierId { get; set; }

    [Required]
    public required PriceDto Price { get; set; }
}
