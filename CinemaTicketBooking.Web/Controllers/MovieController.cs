using AutoMapper;
using CinemaTicketBooking.Application.UseCases;
using CinemaTicketBooking.Web.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBooking.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IGetMoviesUseCase getMoviesUseCase;

    public MovieController(IGetMoviesUseCase getMoviesUseCase, IMapper mapper)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.getMoviesUseCase = getMoviesUseCase ?? throw new ArgumentNullException(nameof(getMoviesUseCase));
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<List<MovieDto>>> List()
    {
        var result = await getMoviesUseCase.ExecuteAsync();
        return Ok(result);
    }
}
