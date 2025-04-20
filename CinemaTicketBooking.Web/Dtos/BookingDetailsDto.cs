using CinemaTicketBooking.Web.Dtos.Movie;
using CinemaTicketBooking.Web.Dtos.Theater;

namespace CinemaTicketBooking.Web.Dtos;

public class BookingDetailsDto
{
    public required BookingDto Booking { get; set; }

    public required CustomerDto Customer { get; set; }

    public required TheaterDto Theater { get; set; }

    public required ScreeningDto Screening { get; set; }

    public required MovieDto Movie { get; set; }

    public required List<SeatReservationDto> SeatReservations { get; set; }

    public required PriceDto TotalPrice { get; set; }
}
