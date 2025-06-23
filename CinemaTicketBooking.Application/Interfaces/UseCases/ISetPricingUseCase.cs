using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public interface ISetPricingUseCase
{
    public Task ExecuteAsync(Pricing pricing);
}
