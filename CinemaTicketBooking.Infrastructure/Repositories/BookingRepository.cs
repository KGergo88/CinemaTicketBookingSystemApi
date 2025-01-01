using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.Infrastructure.Repositories;

internal class BookingRepository : IBookingRepository
{
    private readonly IMapper mapper;
    private readonly CinemaTicketBookingDbContext context;

    public BookingRepository(IMapper mapper, CinemaTicketBookingDbContext context)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddBookingAsync(Booking booking)
    {
        var infraBooking = mapper.Map<BookingEntity>(booking);
        context.Bookings.Add(infraBooking);
        await context.SaveChangesAsync();
    }

    public async Task<Booking?> GetBookingAsync(Guid bookingId)
    {
        var infraBooking = await context.Bookings.SingleOrDefaultAsync(b => b.Id == bookingId);

        if (infraBooking == null)
            return null;

        return mapper.Map<Booking>(infraBooking);
    }

    public async Task UpdateBookingAsync(Booking booking)
    {
        var infraBooking = mapper.Map<BookingEntity>(booking);
        context.Bookings.Update(infraBooking);
        await context.SaveChangesAsync();
    }
}
