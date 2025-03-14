namespace CinemaTicketBooking.Web;

public class StartupException(string message, Exception? innerException = null)
    : Exception(message, innerException) { }
