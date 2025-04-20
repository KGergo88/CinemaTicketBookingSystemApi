using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos.Theater;

public abstract class TheaterDtoBase<TAuditoriumDto>
{
    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Address { get; set; }

    [MinLength(1)]
    public required List<TAuditoriumDto> Auditoriums { get; set; }
}
