namespace CinemaTicketBooking.Web.Dtos.GetAvailableSeats;

public class GetAvailableSeatsResponseDto
{
    public required List<SeatDto> AvailableSeats { get; set; }
}
