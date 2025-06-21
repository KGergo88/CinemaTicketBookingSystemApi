using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Web.Dtos;
using CinemaTicketBooking.Web.Dtos.GetAvailableSeats;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBooking.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScreeningsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IAddScreeningsUseCase addScreeningsUseCase;
    private readonly ISetPricingUseCase setPricingUseCase;
    private readonly IGetAvailableSeatsUseCase getAvailableSeatsUseCase;

    public ScreeningsController(
        IMapper mapper,
        IAddScreeningsUseCase addScreeningsUseCase,
        ISetPricingUseCase setPricingUseCase,
        IGetAvailableSeatsUseCase getAvailableSeatsUseCase)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.addScreeningsUseCase = addScreeningsUseCase ?? throw new ArgumentNullException(nameof(addScreeningsUseCase));
        this.setPricingUseCase = setPricingUseCase ?? throw new ArgumentNullException(nameof(setPricingUseCase));
        this.getAvailableSeatsUseCase = getAvailableSeatsUseCase ?? throw new ArgumentNullException(nameof(getAvailableSeatsUseCase));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddScreenings([FromBody] IEnumerable<ScreeningDto> screeningDtos)
    {
        var screenings = mapper.Map<IEnumerable<Screening>>(screeningDtos);
        await addScreeningsUseCase.ExecuteAsync(screenings);
        return Created();
    }

    [HttpPost("pricing")]
    public async Task<ActionResult> SetPricing([FromBody] PricingDto pricingDto)
    {
        var pricing = mapper.Map<Pricing>(pricingDto);
        await setPricingUseCase.ExecuteAsync(pricing);
        return Ok();
    }

    [HttpGet("{screeningId:guid}/availableSeats")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
