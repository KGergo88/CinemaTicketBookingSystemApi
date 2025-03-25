using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos;

public class AuditoriumDto
{
    [Required]
    public string Name { get; set; }

    [MinLength(1)]
    public List<TierDto> Tiers { get; set; }
}
