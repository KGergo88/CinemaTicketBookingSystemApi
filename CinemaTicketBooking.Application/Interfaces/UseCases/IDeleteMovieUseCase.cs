namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public class DeleteMovieUseCaseException(string message, Exception? innerException)
    : Exception(message, innerException) { }

public interface IDeleteMovieUseCase
{
    public Task ExecuteAsync(Guid movieIdToDelete);
}
