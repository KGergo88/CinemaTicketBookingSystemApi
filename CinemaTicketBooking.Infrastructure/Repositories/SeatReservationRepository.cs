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

    public async Task AddSeatReservationsAsync(IEnumerable<SeatReservation> seatReservations)
    {
        var infraSeatReservations = mapper.Map<List<SeatReservationEntity>>(seatReservations);
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
                                                                 .Where(sr => sr.ScreeningId == screeningId
                                                                              && (sr.Booking!.BookingState == (int)BookingState.NonConfirmed
                                                                                  || sr.Booking.BookingState == (int)BookingState.Confirmed))
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
        await context.SeatReservations.Include(sr => sr.Booking)
                                      .Where(sr => sr.Booking!.BookingState == (int)BookingState.ConfirmationTimeout)
                                      .ExecuteDeleteAsync();
    }

    public async Task<List<Guid>> FindAlreadyReservedSeatIdsAsync(Guid screeningId, IEnumerable<Guid> seatIdsToCheck)
    {
        var reservedSeats = await GetReservedSeatsOfTheScreeningAsync(screeningId);
        var reservedSeatsIds = reservedSeats.Select(rs => rs.Id)
                                            .ToHashSet();
        var alreadyReservedSeatIds = seatIdsToCheck.Where(reservedSeatsIds.Contains)
                                                   .ToList();
        return alreadyReservedSeatIds;
    }

    private async Task<Dictionary<Guid, PricingEntity>> GetPricingEntitiesBySeatId(Guid screeningId)
    {
        var pricingsOfTheScreening = await context.Pricings.Include(p => p.Tier)
                                                           .ThenInclude(t => t!.Seats)
                                                           .Where(p => p.ScreeningId == screeningId)
                                                           .ToListAsync();

        var pricingsBySeats = pricingsOfTheScreening.SelectMany(p => p.Tier!.Seats.Select(s => new { SeatId = s.Id, Pricing = p }))
                                                    .ToDictionary(x => x.SeatId, x => x.Pricing);

        return pricingsBySeats;
    }
}
