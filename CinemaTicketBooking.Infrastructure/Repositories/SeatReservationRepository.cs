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
        var pricingsBySeats = await GetPricingEntitiesBySeatId(screeningId);
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

    public async Task DeleteSeatReservationsAsync(IEnumerable<Guid> seatReservationIds)
    {
        await context.SeatReservations.Where(sr => seatReservationIds.Contains(sr.Id))
                                      .ExecuteDeleteAsync();
    }

    public async Task<List<Seat>> GetReservedSeatsOfTheScreeningAsync(Guid screeningId)
    {
        var reservedSeatEntities = await context.SeatReservations.Include(sr => sr.Seat)
                                                                 .Include(sr => sr.Booking)
                                                                 .Where(sr => sr.ScreeningId == screeningId && IsReserved(sr.Booking.BookingState))
                                                                 .Select(sr => sr.Seat)
                                                                 .ToListAsync();

        var reservedSeats = mapper.Map<List<Seat>>(reservedSeatEntities);
        return reservedSeats;
    }

    public async Task<List<SeatReservation>> GetSeatReservationsOfABookingAsync(Guid bookingId)
    {
        var seatReservationEntities = await context.SeatReservations.Where(sr => sr.BookingId == bookingId)
                                                                    .ToListAsync();

        return mapper.Map<List<SeatReservation>>(seatReservationEntities);
    }

    public async Task ReleaseSeatsOfTimeoutedBookingsAsync()
    {
        await context.SeatReservations.Where(sr => sr.Booking.BookingState == (int)BookingState.ConfirmationTimeout)
                                      .ExecuteDeleteAsync();
    }

    private async Task<Dictionary<Guid, PricingEntity>> GetPricingEntitiesBySeatId(Guid screeningId)
    {
        var pricingsOfTheScreening = await context.Pricings.Include(p => p.Tier)
                                                           .ThenInclude(t => t.Seats)
                                                           .Where(p => p.ScreeningId == screeningId)
                                                           .ToListAsync();

        var pricingsBySeats = pricingsOfTheScreening.SelectMany(p => p.Tier.Seats.Select(s => new { SeatId = s.Id, Pricing = p }))
                                                    .ToDictionary(x => x.SeatId, x => x.Pricing);

        return pricingsBySeats;
    }

    private bool IsReserved(int state)
    {
        return state == (int)BookingState.NonConfirmed || state == (int)BookingState.Confirmed;
    }
}
