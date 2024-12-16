namespace CinemaTicketBooking.Web.Dtos.MakeBooking;

public class BookingRequestDto
{
    public required Guid CustomerId { get; set; }

    public required Guid ScreeningId { get; set; }

    public required List<Guid> SeatsToReserve { get; set; }
}
