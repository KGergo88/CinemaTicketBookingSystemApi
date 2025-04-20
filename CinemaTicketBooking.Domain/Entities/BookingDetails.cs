namespace CinemaTicketBooking.Domain.Entities;

public class BookingDetails
{
    public required Booking Booking { get; set; }

    public required Customer Customer { get; set; }

    public required Theater Theater { get; set;}

    public required Screening Screening { get; set; }

    public required Movie Movie { get; set; }

    public required List<SeatReservation> SeatReservations { get; set; }

    public required Price TotalPrice { get; set; }
}