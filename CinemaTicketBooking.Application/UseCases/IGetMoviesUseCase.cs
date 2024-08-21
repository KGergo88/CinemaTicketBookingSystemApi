using CinemaTicketBooking.Application.Dtos;

namespace CinemaTicketBooking.Application.UseCases;

public interface IGetMoviesUseCase
{
    public Task<List<MovieDto>> ExecuteAsync();
}
