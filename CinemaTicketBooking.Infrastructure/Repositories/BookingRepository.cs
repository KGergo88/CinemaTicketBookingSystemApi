using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Infrastructure.Entities;

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
}
