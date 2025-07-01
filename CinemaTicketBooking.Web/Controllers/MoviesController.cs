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
        var movie = mapper.Map<Movie>(movieDto);
        await addMovieUseCase.ExecuteAsync(movie);
        return Created();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update([FromBody] MovieDto movieDto)
    {
        var movie = mapper.Map<Movie>(movieDto);
        await updateMovieUseCase.ExecuteAsync(movie);
        return Ok();
    }

    [HttpDelete("{movieId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete(Guid movieId)
    {
        await deleteMovieUseCase.ExecuteAsync(movieId);
        return Ok();
    }
}
