namespace CinemaTicketBooking.Domain.Entities;

public class BookingDetails
{
    public Booking Booking { get; set; }

    public Customer Customer { get; set; }

    public Theater Theater { get; set;}

    public Screening Screening { get; set; }

    public Movie Movie { get; set; }

    public List<SeatReservation> SeatReservations { get; set; }

    public Price TotalPrice { get; set; }
}