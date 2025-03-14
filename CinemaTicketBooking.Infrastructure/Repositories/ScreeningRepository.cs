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

    public async Task<Screening> GetScreeningOrNullAsync(Guid id)
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

    public async Task SetPricingAsync(Guid screeningId, Guid tierId, Pricing pricing)
    {
        var infraPricing = await context.Pricings.SingleOrDefaultAsync(p => p.ScreeningId == screeningId
                                                                            && p.TierId == tierId);
        if (infraPricing is null) {
            infraPricing = mapper.Map<PricingEntity>(pricing);
            infraPricing.ScreeningId = screeningId;
            infraPricing.TierId = tierId;
            context.Pricings.Add(infraPricing);
        }

        infraPricing.Price = pricing.Price.Amount;
        infraPricing.Currency = pricing.Price.Currency;


        await context.SaveChangesAsync();
    }
}
