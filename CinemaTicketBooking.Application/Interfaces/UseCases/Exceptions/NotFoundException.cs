namespace CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;

public class NotFoundException(string message, Exception? innerException = null)
    : UseCaseException(message, innerException) { }
