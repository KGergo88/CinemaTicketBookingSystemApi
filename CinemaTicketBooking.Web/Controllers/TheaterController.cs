using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Web.Dtos.Theater;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBooking.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class TheaterController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IAddTheatersUseCase addTheatersUseCase;

    public TheaterController(
        IMapper mapper,
        IAddTheatersUseCase addTheatersUseCase)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.addTheatersUseCase = addTheatersUseCase ?? throw new ArgumentNullException(nameof(addTheatersUseCase));
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> AddTheaters(IEnumerable<TheaterWithoutIdDto> theaterDtos)
    {
        var theaters = mapper.Map<List<Theater>>(theaterDtos);
        await addTheatersUseCase.ExecuteAsync(theaters);
        return Ok();
    }
}
