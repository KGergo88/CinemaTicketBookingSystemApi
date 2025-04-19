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

    public async Task<Booking?> GetBookingOrNullAsync(Guid bookingId)
    {
        var infraBooking = await context.Bookings.SingleOrDefaultAsync(b => b.Id == bookingId);

        if (infraBooking == null)
            return null;

        return mapper.Map<Booking>(infraBooking);
    }

    public async Task SetBookingStateAsync(Guid bookingId, BookingState bookingState)
    {
        await context.Bookings.Where(b => b.Id == bookingId)
                              .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.BookingState, (int)bookingState));
    }
    }

    public async Task TimeoutUnconfirmedBookingsAsync(int timeoutInMinutes)
    {
        if (timeoutInMinutes <= 0)
            throw new BookingRepositoryException($"{nameof(timeoutInMinutes)} shall be greater than zero! Actual value: {timeoutInMinutes}");

        // TODO
        // Instead of doing two round trips to the DB, we should use ExecuteUpdateAsync() here,
        // but for some reason that does not seem to work with our test environment
        // The code for that would look like this:
        //   await context.Bookings.Where(b => b.BookingState == (int)BookingState.NonConfirmed
        //                                     && (b.CreatedOn.AddMinutes(timeoutInMinutes) < DateTimeOffset.UtcNow))
        //                         .ExecuteUpdateAsync(updates => updates.SetProperty(b => b.BookingState, (int)BookingState.ConfirmationTimeout));

        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var infraBookingsToTimeout = await context.Bookings.Where(b => b.BookingState == (int)BookingState.NonConfirmed
                                                                           && (b.CreatedOn.AddMinutes(timeoutInMinutes) < DateTimeOffset.UtcNow))
                                                          .ToListAsync();
            infraBookingsToTimeout.ForEach(b => b.BookingState = (int)BookingState.ConfirmationTimeout);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new BookingRepositoryException(ex.Message);
        }
    }
}
