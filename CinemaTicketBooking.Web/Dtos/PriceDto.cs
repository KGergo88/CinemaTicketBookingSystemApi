using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos;

public class PriceDto
{
    [Required]
    [Range(0, float.MaxValue)]
    public required float Amount { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Currency { get; set; }
}
