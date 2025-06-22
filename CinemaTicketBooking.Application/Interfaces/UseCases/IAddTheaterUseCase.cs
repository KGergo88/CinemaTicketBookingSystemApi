using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public interface IAddTheaterUseCase
{
    public Task ExecuteAsync(Theater theater);
}
