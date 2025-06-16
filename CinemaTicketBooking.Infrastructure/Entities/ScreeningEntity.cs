using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Infrastructure.Entities;

// Indexes
// The combination of the AuditoriumId and the Showtime is unique,
// as there shall be no two screening at the same time in the same auditorium
[Index(nameof(AuditoriumId), nameof(Showtime), IsUnique = true)]
internal class ScreeningEntity
{
    [Required]
    public required Guid Id { get; set; }

    [Required]
    public required Guid AuditoriumId { get; set; }

    public AuditoriumEntity? Auditorium { get; set; }

    [Required]
    public required Guid MovieId { get; set; }

    public MovieEntity? Movie { get; set; }

    [Required]
    public required DateTimeOffset Showtime { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Language { get; set; }

    [MaxLength(50)]
    public string? Subtitles { get; set; }

    public ICollection<SeatReservationEntity> SeatReservations { get; set; } = new List<SeatReservationEntity>();

    public ICollection<PricingEntity> Pricings { get; set; } = new List<PricingEntity>();
}
