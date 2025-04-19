using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.Repositories;

public class BookingRepositoryException(string message, Exception? innerException = null)
    : Exception(message, innerException) { }

public interface IBookingRepository
{
    public Task AddBookingAsync(Booking domainBooking);

    public Task<Booking?> GetBookingOrNullAsync(Guid bookingId);

    public Task UpdateBookingAsync(Booking domainBooking);

    public Task TimeoutUnconfirmedBookingsAsync(int timeoutInMinutes);
}
