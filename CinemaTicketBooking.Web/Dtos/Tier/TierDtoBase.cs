using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos.Tier;

public abstract class TierDtoBase<TSeatDto>
{
    [Required]
    public string Name { get; set; }

    [MinLength(1)]
    public List<TSeatDto> Seats { get; set; }
}
