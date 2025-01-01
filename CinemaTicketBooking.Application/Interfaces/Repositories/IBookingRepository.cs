using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.Repositories;

public interface IBookingRepository
{
    public Task AddBookingAsync(Booking domainBooking);

    public Task<Booking?> GetBookingAsync(Guid bookingId);

    public Task UpdateBookingAsync(Booking domainBooking);
}
