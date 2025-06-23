using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public class AddMovieUseCaseException(string message, Exception? innerException)
    : UseCaseException(message, innerException) { }

public interface IAddMovieUseCase
{
    public Task ExecuteAsync(Movie movies);
}
