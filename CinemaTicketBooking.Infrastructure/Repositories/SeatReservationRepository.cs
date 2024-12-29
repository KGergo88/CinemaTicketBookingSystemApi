using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Domain.Entities;
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

    public async Task<List<Seat>> GetAvailableSeats(Guid screningId)
    {
        var screeningEntity = await context.Screenings.AsSplitQuery()
                                                      .Include(s => s.Auditorium)
                                                      .ThenInclude(a => a.Tiers)
                                                      .ThenInclude(t => t.Seats)
                                                      .Where(s => s.Id == screningId)
                                                      .SingleAsync();

        var allSeatEntities = screeningEntity.Auditorium.Tiers.SelectMany(t => t.Seats)
                                                              .ToDictionary(s => s.Id);

        var reservedSeatEntities = await context.SeatReservations.Include(sr => sr.Seat)
                                                                 .Include(sr => sr.Booking)
                                                                 .Where(sr => sr.ScreeningId == screningId
                                                                              && (sr.Booking.BookingState == (int)BookingState.NonConfirmed
                                                                                  || sr.Booking.BookingState == (int)BookingState.Confirmed))
                                                                 .Select(sr => sr.Seat)
                                                                 .ToDictionaryAsync(s => s.Id);

        foreach (var key in reservedSeatEntities.Keys)
        {
            allSeatEntities.Remove(key);
        }

        var availableSeatEntities = allSeatEntities.Values.ToList();

        return mapper.Map<List<Seat>>(availableSeatEntities);
    }
}
