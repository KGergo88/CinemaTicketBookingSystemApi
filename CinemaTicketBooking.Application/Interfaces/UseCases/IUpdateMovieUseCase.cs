using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public class UpdateMovieUseCaseException(string message, Exception? innerException) : Exception(message, innerException) { }

public interface IUpdateMovieUseCase
{
    public Task ExecuteAsync(Movie movie);
}
