namespace CinemaTicketBooking.Domain.Entities;

public enum BookingState
{
    NonConfirmed,
    ConfirmationTimeout,
    Confirmed,
    Cancelled
}
