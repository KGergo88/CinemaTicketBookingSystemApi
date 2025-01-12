namespace CinemaTicketBooking.Domain.Entities;

public class Price
{
    public required float Amount { get; set; }

    public required string Currency { get; set; }
}
