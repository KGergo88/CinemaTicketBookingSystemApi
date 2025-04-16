namespace CinemaTicketBooking.Web.Dtos;

public class BookingDto
{
    public required int BookingState { get; set; }

    public required Guid CustomerId { get; set; }

    public required Guid ScreeningId { get; set; }

    public required DateTimeOffset CreatedOn { get; set; }
}
