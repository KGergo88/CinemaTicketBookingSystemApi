using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Infrastructure.Entities;

// Indexes
// The combination of the ScreeningId and the TierId is unique as a Tier shall not have multiple pricings for a given screening
// The combination of the ScreeningId and the Currency is unique as there should be only one currency used for a given screening
[Index(nameof(ScreeningId), nameof(TierId), IsUnique = true)]
[Index(nameof(ScreeningId), nameof(Currency), IsUnique = true)]
internal class PricingEntity
{
    [Required]
    public required Guid Id { get; set; }

    [Required]
    public required Guid ScreeningId { get; set; }

    public required ScreeningEntity Screening { get; set; }

    [Required]
    public required Guid TierId { get; set; }

    public required TierEntity Tier { get; set; }

    // Negative prices are not valid
    [Required]
    [Range(0, float.MaxValue)]
    public required float Price { get; set; }

    // Constraints
    //   - A price without a currency does not make sense
    //   - The longest currency name is 30 characters long
    // (Ditikolo Tsa Botshelo Jwa Dikoloto, Botswana)
    [Required]
    [MaxLength(50)]
    public required string Currency { get; set; }
}
