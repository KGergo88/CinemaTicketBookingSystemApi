namespace CinemaTicketBooking.Domain.Entities;

public class SeatReservation
{
    public Guid? Id { get; set; }

    public required Guid BookingId { get; set; }

    // This is a redundant reference as the booking already references the screening,
    // but with this included, we can ensure that the same seat is not reserved multiple times for the same screening
    public required Guid ScreeningId { get; set; }

    public required Guid SeatId { get; set; }

    // Not referencing a pricing as the pricing may change after the reservation but a reservation should keep the same price
    public required Price Price { get; set; }
}
