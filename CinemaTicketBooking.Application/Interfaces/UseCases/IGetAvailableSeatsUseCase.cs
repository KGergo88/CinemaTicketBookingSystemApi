using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public interface IGetAvailableSeatsUseCase
{
    public Task<List<Seat>> ExecuteAsync(Guid screeningId);
}
