using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Web.Dtos.GetAvailableSeats;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBooking.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeatReservationsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IGetAvailableSeatsUseCase getAvailableSeatsUseCase;

    public SeatReservationsController(
        IMapper mapper,
        IGetAvailableSeatsUseCase getAvailableSeatsUseCase)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.getAvailableSeatsUseCase = getAvailableSeatsUseCase ?? throw new ArgumentNullException(nameof(getAvailableSeatsUseCase));
    }

    [HttpGet("[action]")]
    public async Task<ActionResult> GetAvailableSeats(Guid screeningId)
    {
        try
        {
            var availableSeats = await getAvailableSeatsUseCase.ExecuteAsync(screeningId);
            var response = mapper.Map<GetAvailableSeatsResponseDto>(availableSeats);
            return Ok(response);
        }
        catch (GetAvailableSeatsUseCaseException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
