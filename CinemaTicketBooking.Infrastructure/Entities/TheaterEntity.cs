using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Infrastructure.Entities;

// Indexes
// One might often search by the Name
// The combination of the Name and the Address is a unique combination as there are shall be no two theaters on the same address
[Index(nameof(Name))]
[Index(nameof(Name), nameof(Address), IsUnique = true)]
internal class TheaterEntity
{
    [Required]
    public required Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Address { get; set; }

    public ICollection<AuditoriumEntity> Auditoriums { get; set; } = new List<AuditoriumEntity>();
}
