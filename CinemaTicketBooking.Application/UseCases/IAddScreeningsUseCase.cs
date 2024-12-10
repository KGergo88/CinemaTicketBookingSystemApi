using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

public interface IAddScreeningsUseCase
{
    public Task ExecuteAsync(List<Screening> screenings);
}
