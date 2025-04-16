using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos.Theater;

public abstract class TheaterDtoBase<TAuditoriumDto>
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Address { get; set; }

    [MinLength(1)]
    public List<TAuditoriumDto> Auditoriums { get; set; }
}
