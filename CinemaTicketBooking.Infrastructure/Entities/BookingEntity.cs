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
    public required Guid Id { get; set; }

    [Required]
    public required int BookingState { get; set; }

    [Required]
    public required Guid CustomerId { get; set; }

    public CustomerEntity? Customer { get; set; }

    [Required]
    public required Guid ScreeningId { get; set; }

    public ScreeningEntity? Screening { get; set; }

    [Required]
    public required DateTimeOffset CreatedOn { get; set; }
}
