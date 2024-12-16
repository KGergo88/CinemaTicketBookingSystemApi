namespace CinemaTicketBooking.Web.Dtos.MakeBooking;

public class BookingResponseDto
{
    public required Guid BookingId { get; set; }

    public required int BookingState { get; set; }
}
