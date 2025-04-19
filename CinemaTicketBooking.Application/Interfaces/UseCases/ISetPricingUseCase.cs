using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public class SetPricingUseCaseException(string message, Exception? innerException)
    : Exception(message, innerException) { }

public interface ISetPricingUseCase
{
    public Task ExecuteAsync(Pricing pricing);
}
