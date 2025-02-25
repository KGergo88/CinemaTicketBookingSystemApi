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

    public async Task AddSeatReservationsAsync(IEnumerable<Guid> seatIdsToReserve, Guid bookingId, Guid screeningId)
    {
        var pricingsBySeats = await GetPricingEntitiesForSeatIds(screeningId, seatIdsToReserve);
        if (seatIdsToReserve.Any(sitr => !pricingsBySeats.ContainsKey(sitr)))
            throw new SeatReservationRepositoryException("Could not reserve seats as pricing information is not available for some one of them.");

        var infraSeatReservations = seatIdsToReserve.Select(sitr => new SeatReservationEntity()
        {
            Id = Guid.NewGuid(),
            BookingId = bookingId,
            ScreeningId = screeningId,
            SeatId = sitr,
            Price = pricingsBySeats[sitr].Price,
            Currency = pricingsBySeats[sitr].Currency
        });
        context.SeatReservations.AddRange(infraSeatReservations);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            if (databaseBinding.IsUniqueIndexException(ex))
                throw new SeatReservationRepositoryException(
                    "Could not reserve seats as at least one of them seems to be already reserved.", ex);

            throw;
        }
    }

    public async Task<List<Seat>> GetAvailableSeatsAsync(Guid screningId)
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

    public async Task<List<SeatReservation>> GetSeatReservationsOfABookingAsync(Guid bookingId)
    {
        var seatReservationEntities = await context.SeatReservations.Include(sr => sr.Seat)
                                                                    .Include(sr => sr.Screening)
                                                                    .Where(sr => sr.BookingId == bookingId)
                                                                    .ToListAsync();

        return mapper.Map<List<SeatReservation>>(seatReservationEntities);
    }

    public async Task ReleaseSeatsOfTimeoutedBookingsAsync()
    {
        await context.SeatReservations.Where(sr => sr.Booking.BookingState == (int)BookingState.ConfirmationTimeout)
                                      .ExecuteDeleteAsync();
    }

    private async Task<Dictionary<Guid, PricingEntity>> GetPricingEntitiesForSeatIds(Guid screeningId, IEnumerable<Guid> seatIds)
    {
        var pricingsOfTheScreening = await context.Pricings.Include(p => p.Tier)
                                                    .ThenInclude(t => t.Seats)
                                                    .Where(p => p.ScreeningId == screeningId)
                                                    .ToListAsync();

        var pricingsBySeats = pricingsOfTheScreening.SelectMany(p => p.Tier.Seats.Select(s => new { SeatId = s.Id, Pricing = p }))
                                                    .ToDictionary(x => x.SeatId, x => x.Pricing);

        return pricingsBySeats;
    }
}
