using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Infrastructure.Entities;
using CinemaTicketBooking.Infrastructure.DatabaseBindings;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.Infrastructure.Repositories;

internal class SeatReservationRepository : ISeatReservationRepository
{
    private readonly IMapper mapper;
    private readonly IDatabaseBinding databaseBinding;
    private readonly CinemaTicketBookingDbContext context;

    public SeatReservationRepository(IMapper mapper, IDatabaseBinding databaseBinding, CinemaTicketBookingDbContext context)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.databaseBinding = databaseBinding ?? throw new ArgumentNullException(nameof(databaseBinding));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddSeatReservationsAsync(List<Guid> seatsToReserve, Guid bookingId, Guid screeningId)
    {
        var infraSeatReservations = seatsToReserve.Select(str => new SeatReservationEntity()
        {
            Id = Guid.NewGuid(),
            BookingId = bookingId,
            ScreeningId = screeningId,
            SeatId = str
        });
        context.SeatReservations.AddRange(infraSeatReservations);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException dbUpdateException)
        {
            if (databaseBinding.IsUniqueIndexException(dbUpdateException))
                throw new SeatReservationRepositoryException(
                    "Could not reserve seats as at least one of them seems to be already reserved.", dbUpdateException);

            throw;
        }
    }
}
