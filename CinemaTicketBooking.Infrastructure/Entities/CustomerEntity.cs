using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Infrastructure.Entities;

// Indexes
// There shall be no two users with the same E-Mail address
[Index(nameof(Email), IsUnique = true)]
internal class CustomerEntity
{
    [Required]
    public required Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public required string LastName { get; set; }

    [Required]
    [MaxLength(50)]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    public required string Email { get; set; }
}
