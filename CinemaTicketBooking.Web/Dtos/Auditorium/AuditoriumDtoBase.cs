using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos.Auditorium;

public abstract class AuditoriumDtoBase<TTierDto>
{
    [Required]
    public string Name { get; set; }

    [MinLength(1)]
    public List<TTierDto> Tiers { get; set; }
}
