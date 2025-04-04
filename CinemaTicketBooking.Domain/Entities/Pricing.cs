namespace CinemaTicketBooking.Domain.Entities;

public class Pricing
{
    public Guid? Id { get; set; }

    public required Guid ScreeningId { get; set; }

    public required Guid TierId { get; set; }

    public required Price Price { get; set; }
}
