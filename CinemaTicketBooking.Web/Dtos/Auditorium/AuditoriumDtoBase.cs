using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos.Auditorium;

public abstract class AuditoriumDtoBase<TTierDto>
{
    [Required]
    public required string Name { get; set; }

    [MinLength(1)]
    public required List<TTierDto> Tiers { get; set; }
}
