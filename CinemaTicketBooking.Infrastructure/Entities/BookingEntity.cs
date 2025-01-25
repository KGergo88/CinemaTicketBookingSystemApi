using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.Infrastructure.Entities;

// Indexes
// One might often search either by the CustomerId only or by the combination of the BookingState and the CreatedOn
[Index(nameof(CustomerId))]
[Index(nameof(BookingState), nameof(CreatedOn))]
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
