namespace CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;

public class ConflictException(string message, Exception? innerException = null)
    : UseCaseException(message, innerException) { }
