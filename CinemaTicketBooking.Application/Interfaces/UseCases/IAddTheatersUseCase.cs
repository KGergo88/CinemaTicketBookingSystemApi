using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public class AddTheatersUseCaseException(string message, Exception? innerException) : Exception(message, innerException) { }

public interface IAddTheatersUseCase
{
    public Task ExecuteAsync(IEnumerable<Theater> theaters);
}
