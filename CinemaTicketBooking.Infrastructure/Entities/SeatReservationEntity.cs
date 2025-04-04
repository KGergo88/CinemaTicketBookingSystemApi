using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Infrastructure.Entities;

// Indexes
// No need to define indexes here explicitly as foreign keys are indexed by the database by default
// The combination of the ScreeningId and the SeatId is unique as the same seat shall not be booked twice for the same screening
[Index(nameof(ScreeningId), nameof(SeatId), IsUnique = true)]
internal class SeatReservationEntity
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid BookingId { get; set; }

    public BookingEntity Booking { get; set; }

    // This is a redundant reference as the booking already references the screening,
    // but with this included, we can ensure that the same seat is not reserved multiple times for the same screening
    [Required]
    public Guid ScreeningId { get; set; }

    public ScreeningEntity Screening { get; set; }

    [Required]
    public Guid SeatId { get; set; }

    public SeatEntity Seat { get; set; }

    // Not referencing a pricing as the pricing may change after the reservation but a reservation should keep the same price
    // Negative prices are not valid
    [Required]
    [Range(0, float.MaxValue)]
    public float Price { get; set; }

    // Constraints
    //   - A price without a currency does not make sense
    //   - The longest currency name is 30 characters long
    // (Ditikolo Tsa Botshelo Jwa Dikoloto, Botswana)
    [Required]
    [MaxLength(50)]
    public string Currency { get; set; }
}
