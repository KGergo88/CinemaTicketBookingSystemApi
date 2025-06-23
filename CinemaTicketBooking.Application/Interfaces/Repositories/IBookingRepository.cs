using CinemaTicketBooking.Application.Interfaces.Repositories.Exceptions;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.Repositories;

public class BookingRepositoryException(string message, Exception? innerException = null)
    : RepositoryException(message, innerException) { }

public interface IBookingRepository
{
    public Task AddBookingAsync(Booking domainBooking);

    public Task<Booking?> GetBookingOrNullAsync(Guid bookingId);

    public Task SetBookingStateAsync(Guid bookingId, BookingState bookingState);

    public Task DeleteBookingAsync(Guid bookingId);

    public Task TimeoutUnconfirmedBookingsAsync(int timeoutInMinutes);
}
