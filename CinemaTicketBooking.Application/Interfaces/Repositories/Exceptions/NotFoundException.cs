namespace CinemaTicketBooking.Application.Interfaces.Repositories.Exceptions;

public class NotFoundException(string message, Exception? innerException = null)
    : RepositoryException(message, innerException) { }
