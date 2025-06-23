using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public class DeleteMovieUseCaseException(string message, Exception? innerException)
    : UseCaseException(message, innerException) { }

public interface IDeleteMovieUseCase
{
    public Task ExecuteAsync(Guid movieIdToDelete);
}
