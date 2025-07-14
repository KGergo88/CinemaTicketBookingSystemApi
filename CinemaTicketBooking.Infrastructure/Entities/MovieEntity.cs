using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Infrastructure.Entities;

// Indexes
// One might often search either by the Title only or by the combination of the Title and the ReleaseYear
// The combination of the Title and the ReleaseYear is a unique combination as there are shall be no movies with the
// same title released in the same year
[Index(nameof(Title))]
[Index(nameof(Title), nameof(ReleaseYear), IsUnique = true)]
internal class MovieEntity
{
    [Required]
    public required Guid Id { get; set; }

    // Constraints
    //   - Every movie must have a title
    //   - The longest movie title is 197 characters long
    // (Night of the Day of the Dawn of the Son of the Bride of the Return of the Revenge of the Terror of the Attack
    // of the Evil, Mutant, Hellbound, Flesh-Eating Subhumanoid Zombified Living Dead, Part 3, 2005)
    [Required]
    [MaxLength(250)]
    public required string Title { get; set; }

    // Null means that the release is not known
    // Constraints
    //   - The year must be a 4 digit number
    [Precision(4)]
    public int? ReleaseYear { get; set; } = null;

    public required string Description { get; set; } = "";

    // Movies without duration are not valid
    // The longest movie Resan (The Journey), 1987 is 52380 seconds long
    [Precision(5)]
    [Range(1, 99999)]
    public required int DurationInSeconds { get; set; }

    public ICollection<GenreEntity> Genres { get; set; } = new List<GenreEntity>();
}
