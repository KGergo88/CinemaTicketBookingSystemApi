using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public class SetPricingUseCaseException(string message, Exception? innerException)
    : UseCaseException(message, innerException) { }

public interface ISetPricingUseCase
{
    public Task ExecuteAsync(Pricing pricing);
}
