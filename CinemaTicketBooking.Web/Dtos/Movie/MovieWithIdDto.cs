using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos.Movie;

public class MovieWithIdDto : MovieWithoutIdDto
{
    [Required]
    public Guid Id { get; set; }
}
