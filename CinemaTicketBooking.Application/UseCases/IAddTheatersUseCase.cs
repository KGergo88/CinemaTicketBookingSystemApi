using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

public interface IAddTheatersUseCase
{
    public Task ExecuteAsync(List<Theater> theaters);
}
