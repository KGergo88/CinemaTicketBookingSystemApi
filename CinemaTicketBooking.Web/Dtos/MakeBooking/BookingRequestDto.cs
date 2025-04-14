using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos.MakeBooking;

public class BookingRequestDto
{
    [Required]
    public required Guid CustomerId { get; set; }

    [Required]
    public required Guid ScreeningId { get; set; }

    [MinLength(1)]
    public required List<Guid> SeatsToReserve { get; set; }
}
