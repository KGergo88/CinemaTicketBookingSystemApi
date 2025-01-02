using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Web.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBooking.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IGetMoviesUseCase getMoviesUseCase;
    private readonly IAddMoviesUseCase addMoviesUseCase;
    private readonly IUpdateMovieUseCase updateMovieUseCase;
    private readonly IDeleteMovieUseCase deleteMoviesUseCase;

    public MovieController(
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

    [HttpGet("[action]")]
    public async Task<ActionResult<List<MovieDto>>> List()
    {
        var movies = await getMoviesUseCase.ExecuteAsync();
        var movieDtos = mapper.Map<List<MovieDto>>(movies);
        return Ok(movieDtos);
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> Add(List<MovieDto> moviesDtos)
    {
        var movies = mapper.Map<List<Movie>>(moviesDtos);
        await addMoviesUseCase.ExecuteAsync(movies);
        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> Update(MovieDto movieDto)
    {
        var movie = mapper.Map<Movie>(movieDto);
        await updateMovieUseCase.ExecuteAsync(movie);
        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> Delete(List<Guid> movieIdsToDelete)
    {
        await deleteMoviesUseCase.ExecuteAsync(movieIdsToDelete);
        return Ok();
    }
}
