using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Infrastructure.Entities;

internal class AuditoriumEntity
{
    [Required]
    public Guid Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    public Guid TheaterId { get; set; }

    public TheaterEntity Theater { get; set; }

    public ICollection<TierEntity> Tiers { get; set; }
}
