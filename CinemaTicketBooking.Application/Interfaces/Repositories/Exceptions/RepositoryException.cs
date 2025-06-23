namespace CinemaTicketBooking.Application.Interfaces.Repositories.Exceptions;

public class RepositoryException(string message, Exception? innerException = null)
    : Exception(message, innerException) { }
