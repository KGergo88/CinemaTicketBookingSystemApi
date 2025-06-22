using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public interface IAddScreeningUseCase
{
    public Task ExecuteAsync(Screening screening);
}
