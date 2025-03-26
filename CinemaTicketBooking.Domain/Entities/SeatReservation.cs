namespace CinemaTicketBooking.Domain.Entities;

public class SeatReservation
{
    public Guid? Id { get; set; }

    public required Guid BookingId { get; set; }

    public required Guid ScreeningId { get; set; }

    public required Guid SeatId { get; set; }

    public required Price Price { get; set; }
}
