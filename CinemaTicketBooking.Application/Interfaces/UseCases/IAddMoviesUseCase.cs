using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public class AddMoviesUseCaseException(string message, Exception? innerException) : Exception(message, innerException) { }

public interface IAddMoviesUseCase
{
    public Task ExecuteAsync(IEnumerable<Movie> movies);
}
