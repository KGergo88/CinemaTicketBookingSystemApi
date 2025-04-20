namespace CinemaTicketBooking.Domain.Entities;

public class Tier
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required List<Seat> Seats { get; set; }
}
