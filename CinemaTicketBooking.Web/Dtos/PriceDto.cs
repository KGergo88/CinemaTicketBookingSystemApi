using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos;

public class PriceDto
{
    [Required]
    [Range(0, float.MaxValue)]
    public float Amount { get; set; }

    [Required]
    [MaxLength(50)]
    public string Currency { get; set; }
}
