namespace CinemaTicketBooking.Domain.Entities;

public class BookingDetails
{
    public Booking Booking { get; set; }

    public Theater Theater { get; set;}

    public Screening Screening { get; set; }

    public List<SeatReservation> SeatReservations { get; set; }

    public float TotalPrice { get; set; }

    public string Currency { get; set; }

    public DateTimeOffset CreatedOn { get; set; }
}
