using CinemaTicketBooking.Application.Dtos;
using CinemaTicketBooking.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBooking.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    private readonly IGetMoviesUseCase getMoviesUseCase;

    public MovieController(IGetMoviesUseCase getMoviesUseCase)
    {
        this.getMoviesUseCase = getMoviesUseCase ?? throw new ArgumentNullException(nameof(getMoviesUseCase));
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<List<MovieDto>>> List()
    {
        var result = await getMoviesUseCase.ExecuteAsync();
        return Ok(result);
    }
}
