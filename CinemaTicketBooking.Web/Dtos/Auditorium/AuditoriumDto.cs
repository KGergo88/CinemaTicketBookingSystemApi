using CinemaTicketBooking.Web.Dtos.Tier;
using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos.Auditorium;

public class AuditoriumDto : AuditoriumDtoBase<TierDto>
{
    [Required]
    public Guid Id { get; set; }
}
