using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.Repositories.Exceptions;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.Infrastructure.Repositories;

internal class ScreeningRepository : IScreeningRepository
{
    private readonly IMapper mapper;
    private readonly CinemaTicketBookingDbContext context;

    public ScreeningRepository(IMapper mapper, CinemaTicketBookingDbContext context)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Screening?> GetScreeningOrNullAsync(Guid id)
    {
        var infraScreening = await context.Screenings.SingleOrDefaultAsync(s => s.Id == id);
        return mapper.Map<Screening>(infraScreening);
    }

    public async Task AddScreeningsAsync(IEnumerable<Screening> domainScreenings)
    {
        var infraScreenings = mapper.Map<IEnumerable<ScreeningEntity>>(domainScreenings);
        context.Screenings.AddRange(infraScreenings);
        await context.SaveChangesAsync();
    }

    public async Task SetPricingAsync(Pricing pricing)
    {
        var infraPricing = await context.Pricings.SingleOrDefaultAsync(p => p.ScreeningId == pricing.ScreeningId
                                                                            && p.TierId == pricing.TierId);
        if (infraPricing is null)
        {
            infraPricing = mapper.Map<PricingEntity>(pricing);
            context.Pricings.Add(infraPricing);
        }
        else
        {
            infraPricing.Price = pricing.Price.Amount;
            infraPricing.Currency = pricing.Price.Currency;
        }

        await context.SaveChangesAsync();
    }

    public async Task<Dictionary<Guid, Pricing>> GetPricingsBySeatIdAsync(Guid screeningId)
    {
        var infraPricings = await context.Pricings.Include(p => p.Tier)
                                                  .ThenInclude(t => t!.Seats)
                                                  .Where(p => p.ScreeningId == screeningId)
                                                  .ToListAsync();

        var infraPricingsBySeatId = infraPricings.SelectMany(p => p.Tier!.Seats.Select(s => new { SeatId = s.Id, InfraPricing = p }))
                                                 .ToDictionary(x => x.SeatId, x => x.InfraPricing);

        var pricingsBySeatId = mapper.Map<Dictionary<Guid, Pricing>>(infraPricingsBySeatId);
        return pricingsBySeatId;
    }

    public async Task<List<Seat>> GetAllSeatsOfTheScreeningAsync(Guid screeningId)
    {
        var infraScreening = await context.Screenings.AsSplitQuery()
                                                     .Include(s => s.Auditorium)
                                                     .ThenInclude(a => a!.Tiers)
                                                     .ThenInclude(t => t.Seats)
                                                     .Where(s => s.Id == screeningId)
                                                     .SingleOrDefaultAsync();

        if (infraScreening is null)
            throw new NotFoundException($"No screening was found with the ID: {screeningId}");

        var allInfraSeats = infraScreening.Auditorium!.Tiers.SelectMany(t => t.Seats);
        var allSeats = mapper.Map<List<Seat>>(allInfraSeats);
        return allSeats;
    }

    public async Task<List<Guid>> FindNotExistingSeatIdsAsync(Guid screeningId, IEnumerable<Guid> seatIdsToCheck)
    {
        if (!seatIdsToCheck.Any())
            return [];

        var seatsOfTheScreening = await GetAllSeatsOfTheScreeningAsync(screeningId);
        if (seatsOfTheScreening is null || !seatsOfTheScreening.Any())
            throw new NotFoundException($"No seats were found for the screening! ScreeningId: {screeningId}");

        var seatIdsOfTheScreening = seatsOfTheScreening.Select(sots => sots.Id)
                                                       .ToHashSet();
        var notExistingSeatIds = seatIdsToCheck.Where(sitc => !seatIdsOfTheScreening.Contains(sitc))
                                               .ToList();
        return notExistingSeatIds;
    }

    public async Task<List<Guid>> FindScreeningIdsInTimeFrameAsync(Guid auditoriumId, DateTimeOffset timeFrameStart, TimeSpan timeFrameDuration)
    {
        if (timeFrameDuration <= TimeSpan.Zero)
            throw new RepositoryException($"{nameof(timeFrameDuration)} shall be greater than zero! Actual value: {timeFrameDuration}");

        var timeFrameEnd = timeFrameStart.Add(timeFrameDuration);
        var screeningIds = await (
            from screening in context.Screenings
            join movie in context.Movies on screening.MovieId equals movie.Id
            let screeningStart = screening.Showtime
            let screeningEnd = screening.Showtime.AddSeconds(movie.DurationInSeconds)
            where screening.AuditoriumId == auditoriumId
                  // Screening is inside the timeframe if
                  && (
                      // Screening ends inside the timeframe
                      (timeFrameStart < screeningEnd && screeningEnd < timeFrameEnd)
                      // Screening starts inside or at the same time as the timeframe
                      || (timeFrameStart <= screeningStart && screeningStart < timeFrameEnd)
                      // Screening completely overlaps the timeframe (starts before it and ends after it)
                      || (screeningStart < timeFrameStart && timeFrameEnd < screeningEnd)
                  )
            select screening.Id
        ).ToListAsync();

        return screeningIds;
    }
}
