namespace CinemaTicketBooking.Domain.Entities;

public class Pricing
{
    public Guid? Id { get; set; }

    public required Screening Screening { get; set; }

    public required Tier Tier { get; set; }

    public required Price Price { get; set; }
}
