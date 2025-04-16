using CinemaTicketBooking.Web.Dtos.Auditorium;
using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos.Theater;

public class TheaterDto : TheaterDtoBase<AuditoriumDto>
{
    [Required]
    public Guid Id { get; set; }
}
