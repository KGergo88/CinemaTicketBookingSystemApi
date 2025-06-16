using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Infrastructure.Entities;

// Indexes
// One might often search either by the Name which must be unique to avoid duplicated genres.
// A movie with multiple genres like Horror-Comedy should reference multiple Genre entities.
[Index(nameof(Name), IsUnique = true)]
internal class GenreEntity
{
    // Constraints
    //   - Every Genre must have a name
    //   - One of the longest genre name is 21 characters (Phychological Thriller)
    [Key]
    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }

    public ICollection<MovieEntity> Movies { get; set; } = new List<MovieEntity>();
}
