using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos.Movie;

public abstract class MovieDtoBase
{
    // Constraints
    //   - Every movie must have a title
    //   - The longest movie title is 197 characters long
    // (Night of the Day of the Dawn of the Son of the Bride of the Return of the Revenge of the Terror of the Attack
    // of the Evil, Mutant, Hellbound, Flesh-Eating Subhumanoid Zombified Living Dead, Part 3, 2005)
    [Required]
    [MaxLength(250)]
    public string Title { get; set; }

    [Range(0, 9999, ErrorMessage = $"The {nameof(ReleaseYear)} must be a 4-digit number.")]
    public int? ReleaseYear { get; set; }

    public string Description { get; set; } = "";

    // The longest movie Resan (The Journey), 1987 is 52380 seconds long
    [Range(0, 99999, ErrorMessage = $"The {nameof(DurationInSeconds)} must be a 5-digit number.")]
    public int DurationInSeconds { get; set; }

    public List<string> Genres { get; set; } = [];
}
