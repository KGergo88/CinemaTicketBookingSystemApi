using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories;
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
        if (infraPricing is null) {
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

    public async Task<List<Seat>> GetAllSeatsOfTheScreeningAsync(Guid screeningId)
    {
        var infraScreening = await context.Screenings.AsSplitQuery()
                                                     .Include(s => s.Auditorium)
                                                     .ThenInclude(a => a.Tiers)
                                                     .ThenInclude(t => t.Seats)
                                                     .Where(s => s.Id == screeningId)
                                                     .SingleOrDefaultAsync();

        if (infraScreening is null)
            throw new SeatReservationRepositoryException("The requested screening entity does not exist!");

        var allInfraSeats = infraScreening.Auditorium.Tiers.SelectMany(t => t.Seats);
        var allSeats = mapper.Map<List<Seat>>(allInfraSeats);
        return allSeats;
    }
}
