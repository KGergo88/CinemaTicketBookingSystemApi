namespace CinemaTicketBooking.Application.Interfaces.Repositories.Exceptions;

public class DuplicateException(string message, Exception? innerException = null)
    : RepositoryException(message, innerException) { }
