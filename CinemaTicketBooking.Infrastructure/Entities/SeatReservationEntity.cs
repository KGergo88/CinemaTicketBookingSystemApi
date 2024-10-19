using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Infrastructure.Entities;

// Indexes
// No need to define indexes here explicitly as foreign keys are indexed by the database by default
// The combination of the SreeningId and the SeatId is unique as the same seat shall not be booked twice for the same screening
[Index(nameof(ScreeningId), nameof(SeatId), IsUnique = true)]
internal class SeatReservationEntity
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid BookingId { get; set; }

    public BookingEntity Booking { get; set; }

    [Required]
    public Guid ScreeningId { get; set; }

    public ScreeningEntity Screening { get; set; }

    [Required]
    public Guid SeatId { get; set; }

    public SeatEntity Seat { get; set; }
}
