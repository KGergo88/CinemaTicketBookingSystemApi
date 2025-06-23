using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public class GetAvailableSeatsUseCaseException(string message, Exception? innerException = null)
    : UseCaseException(message, innerException) { }

public interface IGetAvailableSeatsUseCase
{
    public Task<List<Seat>> ExecuteAsync(Guid screeningId);
}
