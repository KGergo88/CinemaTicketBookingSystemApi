namespace CinemaTicketBooking.Domain.Entities;

public class Seat
{
    public Guid Id { get; set; }

    public required int Row { get; set; }

    public required int Column { get; set; }
}
