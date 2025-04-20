namespace CinemaTicketBooking.Domain.Entities;

public class Booking
{
    public Guid Id { get; set; }

    public required BookingState BookingState { get; set; }

    public required Guid CustomerId { get; set; }

    public required Guid ScreeningId { get; set; }

    public required DateTimeOffset CreatedOn { get; set; }
}
