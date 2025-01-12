namespace CinemaTicketBooking.Web.Dtos.GetBookingDetails;

public class GetBookingDetailsResponseDto
{
    MovieDto Movie { get; set; }

    ScreeningDto Screening { get; set; }

    List<SeatReservationDto> SeatReservations { get; set; }

    PriceDto TotalPrice { get; set; }
}
