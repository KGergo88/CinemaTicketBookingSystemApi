using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Infrastructure.Entities;

// Indexes
// The combination of the TierId and the Row and the Colum is unique as the same seat shall not exist twice in the same Tier
[Index(nameof(TierId), nameof(Row), nameof(Column), IsUnique = true)]
internal class SeatEntity
{
    [Required]
    public required Guid Id { get; set; }

    [Required]
    public required Guid TierId { get; set; }

    public TierEntity? Tier { get; set; }

    // Negative column numbers are not valid
    [Required]
    [Range(0, int.MaxValue)]
    public required int Row { get; set; }

    // Negative column numbers are not valid
    [Required]
    [Range(0, int.MaxValue)]
    public required int Column { get; set; }

    public ICollection<SeatReservationEntity> SeatReservations { get; set; } = new List<SeatReservationEntity>();
}
