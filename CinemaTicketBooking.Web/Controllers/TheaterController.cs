using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;
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
        IAddTheatersUseCase addTheaterUseCase)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.addTheatersUseCase = addTheaterUseCase ?? throw new ArgumentNullException(nameof(addTheaterUseCase));
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> AddTheaters(List<Dtos.AddTheater.TheaterDto> theaterDtos)
    {
        var theaters = mapper.Map<List<Theater>>(theaterDtos);
        await addTheatersUseCase.ExecuteAsync(theaters);
        return Ok();
    }
}
