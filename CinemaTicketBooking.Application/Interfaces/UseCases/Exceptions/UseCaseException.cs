namespace CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;

public class UseCaseException(string message, Exception? innerException = null)
    : Exception(message, innerException) { }
