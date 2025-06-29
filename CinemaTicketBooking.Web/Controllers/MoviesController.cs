using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;
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
    private readonly IAddMovieUseCase addMovieUseCase;
    private readonly IUpdateMovieUseCase updateMovieUseCase;
    private readonly IDeleteMovieUseCase deleteMovieUseCase;

    public MoviesController(
        IMapper mapper,
        IGetMoviesUseCase getMoviesUseCase,
        IAddMovieUseCase addMovieUseCase,
        IUpdateMovieUseCase updateMovieUseCase,
        IDeleteMovieUseCase deleteMovieUseCase)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.getMoviesUseCase = getMoviesUseCase ?? throw new ArgumentNullException(nameof(getMoviesUseCase));
        this.addMovieUseCase = addMovieUseCase ?? throw new ArgumentNullException(nameof(addMovieUseCase));
        this.updateMovieUseCase = updateMovieUseCase ?? throw new ArgumentNullException(nameof(updateMovieUseCase));
        this.deleteMovieUseCase = deleteMovieUseCase ?? throw new ArgumentNullException(nameof(deleteMovieUseCase));
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
    public async Task<ActionResult> Add([FromBody] MovieWithoutIdDto movieDto)
    {
        try
        {
            var movie = mapper.Map<Movie>(movieDto);
            await addMovieUseCase.ExecuteAsync(movie);
            return Created();
        }
        catch (ConflictException ex)
        {
            return Conflict(ex.Message);
        }
        catch (UseCaseException ex)
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
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UseCaseException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete([FromBody] Guid movieIdToDelete)
    {
        try
        {
            await deleteMovieUseCase.ExecuteAsync(movieIdToDelete);
            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (DeleteMovieUseCaseException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
