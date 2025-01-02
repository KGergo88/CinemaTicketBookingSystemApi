using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public interface IAddScreeningsUseCase
{
    public Task ExecuteAsync(List<Screening> screenings);
}
