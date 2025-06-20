using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Web.Dtos.Movie;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBooking.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IGetMoviesUseCase getMoviesUseCase;
    private readonly IAddMoviesUseCase addMoviesUseCase;
    private readonly IUpdateMovieUseCase updateMovieUseCase;
    private readonly IDeleteMovieUseCase deleteMoviesUseCase;

    public MoviesController(
        IMapper mapper,
        IGetMoviesUseCase getMoviesUseCase,
        IAddMoviesUseCase addMoviesUseCase,
        IUpdateMovieUseCase updateMovieUseCase,
        IDeleteMovieUseCase deleteMoviesUseCase)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.getMoviesUseCase = getMoviesUseCase ?? throw new ArgumentNullException(nameof(getMoviesUseCase));
        this.addMoviesUseCase = addMoviesUseCase ?? throw new ArgumentNullException(nameof(addMoviesUseCase));
        this.updateMovieUseCase = updateMovieUseCase ?? throw new ArgumentNullException(nameof(updateMovieUseCase));
        this.deleteMoviesUseCase = deleteMoviesUseCase ?? throw new ArgumentNullException(nameof(deleteMoviesUseCase));
    }

    [HttpGet]
    public async Task<ActionResult<List<MovieDto>>> List()
    {
        var movies = await getMoviesUseCase.ExecuteAsync();
        var movieDtos = mapper.Map<List<MovieDto>>(movies);
        return Ok(movieDtos);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Add([FromBody] IEnumerable<MovieWithoutIdDto> movieDtos)
    {
        try
        {
            var movies = mapper.Map<List<Movie>>(movieDtos);
            await addMoviesUseCase.ExecuteAsync(movies);
            return Created();
        }
        catch (AddMoviesUseCaseException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update([FromBody] MovieDto movieDto)
    {
        try
        {
            var movie = mapper.Map<Movie>(movieDto);
            await updateMovieUseCase.ExecuteAsync(movie);
            return Ok();
        }
        catch (UpdateMovieUseCaseException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete([FromBody] IEnumerable<Guid> movieIdsToDelete)
    {
        try
        {
            await deleteMoviesUseCase.ExecuteAsync(movieIdsToDelete);
            return Ok();
        }
        catch (DeleteMoviesUseCaseException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
