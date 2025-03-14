using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Web.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBooking.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ScreeningController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IAddScreeningsUseCase addScreeningsUseCase;

    public ScreeningController(
        IMapper mapper,
        IAddScreeningsUseCase addScreeningsUseCase)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.addScreeningsUseCase = addScreeningsUseCase ?? throw new ArgumentNullException(nameof(addScreeningsUseCase));
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> AddScreenings(IEnumerable<ScreeningDto> screeningDtos)
    {
        var screenings = mapper.Map<IEnumerable<Screening>>(screeningDtos);
        await addScreeningsUseCase.ExecuteAsync(screenings);
        return Ok();
    }
}
