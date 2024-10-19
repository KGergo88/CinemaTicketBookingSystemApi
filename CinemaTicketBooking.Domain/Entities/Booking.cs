namespace CinemaTicketBooking.Domain.Entities;

public class Booking
{
    public Guid? Id { get; set; }

    public required BookingState BookingState { get; set; }

    public required Customer Customer { get; set; }

    public required DateTimeOffset CreatedOn { get; set; }
}
