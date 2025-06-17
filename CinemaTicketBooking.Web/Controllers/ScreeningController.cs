using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Web.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBooking.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScreeningsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IAddScreeningsUseCase addScreeningsUseCase;
    private readonly ISetPricingUseCase setPricingUseCase;

    public ScreeningsController(
        IMapper mapper,
        IAddScreeningsUseCase addScreeningsUseCase,
        ISetPricingUseCase setPricingUseCase)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.addScreeningsUseCase = addScreeningsUseCase ?? throw new ArgumentNullException(nameof(addScreeningsUseCase));
        this.setPricingUseCase = setPricingUseCase ?? throw new ArgumentNullException(nameof(setPricingUseCase));
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> AddScreenings(IEnumerable<ScreeningDto> screeningDtos)
    {
        var screenings = mapper.Map<IEnumerable<Screening>>(screeningDtos);
        await addScreeningsUseCase.ExecuteAsync(screenings);
        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> SetPricing(PricingDto pricingDto)
    {
        var pricing = mapper.Map<Pricing>(pricingDto);
        await setPricingUseCase.ExecuteAsync(pricing);
        return Ok();
    }
}
