namespace CinemaTicketBooking.Domain.Entities;

public class SeatReservation
{
    public Guid? Id { get; set; }

    public required Booking Booking { get; set; }

    public required Screening Screening { get; set; }

    public required Seat Seat { get; set; }
}
