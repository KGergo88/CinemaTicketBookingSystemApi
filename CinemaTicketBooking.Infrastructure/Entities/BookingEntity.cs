using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Infrastructure.Entities;

internal class BookingEntity
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public int BookingState { get; set; }

    [Required]
    public Guid CustomerId { get; set; }

    public CustomerEntity Customer { get; set; }

    [Required]
    public DateTimeOffset CreatedOn { get; set; }
}
