using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Infrastructure.Entities;

// Indexes
// The combination of the TierId and the Row and the Colum is a unique as the same seat shall not exist twice in the same Tier
[Index(nameof(TierId), nameof(Row), nameof(Column), IsUnique = true)]
internal class SeatEntity
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid TierId { get; set; }

    public TierEntity Tier { get; set; }

    // Negative column numbers are not valid
    [Required]
    [Range(0, int.MaxValue)]
    public int Row { get; set; }

    // Negative column numbers are not valid
    [Required]
    [Range(0, int.MaxValue)]
    public int Column { get; set; }

    public ICollection<SeatReservationEntity> SeatReservations { get; set; }
}
