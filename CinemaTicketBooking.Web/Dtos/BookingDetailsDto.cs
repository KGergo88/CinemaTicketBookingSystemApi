using CinemaTicketBooking.Web.Dtos.Movie;
using CinemaTicketBooking.Web.Dtos.Theater;

namespace CinemaTicketBooking.Web.Dtos;

public class BookingDetailsDto
{
    public BookingDto Booking { get; set; }

    public CustomerDto Customer { get; set; }

    public TheaterDto Theater { get; set; }

    public ScreeningDto Screening { get; set; }

    public MovieDto Movie { get; set; }

    public List<SeatReservationDto> SeatReservations { get; set; }

    public PriceDto TotalPrice { get; set; }
}
