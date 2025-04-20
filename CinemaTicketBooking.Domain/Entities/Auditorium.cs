namespace CinemaTicketBooking.Domain.Entities;

public class Auditorium
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required List<Tier> Tiers { get; set; }
}
