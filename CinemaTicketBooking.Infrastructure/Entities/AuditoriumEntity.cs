using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Infrastructure.Entities;

internal class AuditoriumEntity
{
    [Required]
    public required Guid Id { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    public required Guid TheaterId { get; set; }

    public TheaterEntity? Theater { get; set; }

    public ICollection<TierEntity> Tiers { get; set; } = new List<TierEntity>();

    [Range(0, int.MaxValue)]
    public required int MinimumCleanupDurationInSeconds { get; set; }
}
