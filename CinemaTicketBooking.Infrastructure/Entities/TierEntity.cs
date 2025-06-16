using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Infrastructure.Entities;

// Indexes
// One might often search by the Name property in combination with a specific Auditorium
// The combination of the Name and the AuditoriumId shall be unique as no auditorium has two tiers with the same name
[Index(nameof(Name), nameof(AuditoriumId), IsUnique = true)]
internal class TierEntity
{
    [Required]
    public required Guid Id { get; set; }

    // Constraints
    //   - Every Tier must have a name, like Normal, Balkony...etc
    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }

    [Required]
    public required Guid AuditoriumId { get; set; }

    public AuditoriumEntity? Auditorium { get; set; }

    public ICollection<SeatEntity> Seats { get; set; } = new List<SeatEntity>();

    public PricingEntity? Pricing { get; set; }
}
