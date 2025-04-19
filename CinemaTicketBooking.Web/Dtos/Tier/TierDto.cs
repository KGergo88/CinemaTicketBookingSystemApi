using CinemaTicketBooking.Web.Dtos.Seat;
using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBooking.Web.Dtos.Tier;

public class TierDto : TierDtoBase<SeatDto>
{
    [Required]
    public Guid Id { get; set; }
}
