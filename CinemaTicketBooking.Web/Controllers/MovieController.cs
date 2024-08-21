using CinemaTicketBooking.Application.Dtos;
using CinemaTicketBooking.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBooking.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController(IGetMoviesUseCase getMoviesUseCase) : ControllerBase
{
    private readonly IGetMoviesUseCase getMoviesUseCase = getMoviesUseCase;

    [HttpGet("[action]")]
    public async Task<ActionResult<List<MovieDto>>> List()
    {
        var result = await getMoviesUseCase.ExecuteAsync();
        return Ok(result);
    }
}
