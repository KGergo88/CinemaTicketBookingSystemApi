using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Web.Dtos.Theater;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBooking.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TheatersController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IAddTheatersUseCase addTheatersUseCase;

    public TheatersController(
        IMapper mapper,
        IAddTheatersUseCase addTheatersUseCase)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.addTheatersUseCase = addTheatersUseCase ?? throw new ArgumentNullException(nameof(addTheatersUseCase));
    }

    [HttpPost]
    public async Task<ActionResult> AddTheaters([FromBody] IEnumerable<TheaterWithoutIdDto> theaterDtos)
    {
        var theaters = mapper.Map<List<Theater>>(theaterDtos);
        await addTheatersUseCase.ExecuteAsync(theaters);
        return Ok();
    }
}
