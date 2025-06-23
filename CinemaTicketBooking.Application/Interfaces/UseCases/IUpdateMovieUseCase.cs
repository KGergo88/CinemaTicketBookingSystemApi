using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public class UpdateMovieUseCaseException(string message, Exception? innerException)
    : UseCaseException(message, innerException) { }

public interface IUpdateMovieUseCase
{
    public Task ExecuteAsync(Movie movie);
}
