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
    private readonly IAddTheaterUseCase addTheaterUseCase;

    public TheatersController(
        IMapper mapper,
        IAddTheaterUseCase addTheaterUseCase)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.addTheaterUseCase = addTheaterUseCase ?? throw new ArgumentNullException(nameof(addTheaterUseCase));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Add([FromBody] TheaterWithoutIdDto theaterDto)
    {
        var theater = mapper.Map<Theater>(theaterDto);
        await addTheaterUseCase.ExecuteAsync(theater);
        return Created();
    }
}
