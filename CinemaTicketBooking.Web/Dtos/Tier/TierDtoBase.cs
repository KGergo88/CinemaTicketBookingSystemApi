using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos.Tier;

public abstract class TierDtoBase<TSeatDto>
{
    [Required]
    public required string Name { get; set; }

    [MinLength(1)]
    public required List<TSeatDto> Seats { get; set; }
}
