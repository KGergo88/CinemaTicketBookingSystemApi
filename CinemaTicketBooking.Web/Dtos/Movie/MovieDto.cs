using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos.Movie;

public class MovieDto : MovieDtoBase
{
    [Required]
    public Guid Id { get; set; }
}
