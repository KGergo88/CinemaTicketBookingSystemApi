using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.Repositories;

public interface IBookingRepository
{
    public Task AddBookingAsync(Booking domainBooking);
}
