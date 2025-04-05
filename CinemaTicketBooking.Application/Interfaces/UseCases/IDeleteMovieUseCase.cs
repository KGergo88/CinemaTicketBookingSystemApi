namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public class DeleteMoviesUseCaseException(string message, Exception? innerException)
    : Exception(message, innerException) { }

public interface IDeleteMovieUseCase
{
    public Task ExecuteAsync(IEnumerable<Guid> movieIdsToDelete);
}
