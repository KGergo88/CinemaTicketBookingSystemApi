using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.Infrastructure.Repositories;

internal class TheaterRepository : ITheaterRepository
{
    private readonly IMapper mapper;
    private readonly CinemaTicketBookingDbContext context;

    public TheaterRepository(IMapper mapper, CinemaTicketBookingDbContext context)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Theater?> GetTheaterOrNullAsync(string name, string address)
    {
        var infraTheater = await context.Theaters.Include(t => t.Auditoriums)
                                                 .ThenInclude(a => a.Tiers)
                                                 .ThenInclude(t => t.Seats)
                                                 .SingleOrDefaultAsync(t => t.Name == name
                                                                            && t.Address == address);

        return mapper.Map<Theater>(infraTheater);
    }

    public async Task AddTheatersAsync(IEnumerable<Theater> domainTheaters)
    {
        var infraTheaters = mapper.Map<IList<TheaterEntity>>(domainTheaters);
        context.Theaters.AddRange(infraTheaters);
        await context.SaveChangesAsync();
    }

    public async Task<Theater> GetTheaterOfAScreeningAsync(Guid screeningId)
    {
        var infraScreening = await context.Screenings.Include(s => s.Auditorium)
                                                     .ThenInclude(a => a!.Theater)
                                                     .ThenInclude(t => t!.Auditoriums)
                                                     .ThenInclude(a => a.Tiers)
                                                     .ThenInclude(t => t.Seats)
                                                     .SingleAsync(s => s.Id == screeningId);

        return mapper.Map<Theater>(infraScreening.Auditorium!.Theater);
    }

    public async Task<Auditorium> GetAuditoriumOrNullAsync(Guid auditoriumId)
    {
        var infraAuditorium = await context.Auditoriums.Include(a => a!.Theater)
                                                       .ThenInclude(t => t!.Auditoriums)
                                                       .ThenInclude(a => a.Tiers)
                                                       .ThenInclude(t => t.Seats)
                                                       .SingleOrDefaultAsync(a => a.Id == auditoriumId);

        return mapper.Map<Auditorium>(infraAuditorium);
    }

    public async Task<Tier> GetTierOrNullAsync(Guid tierId)
    {
        var infraTier = await context.Tiers.Include(t => t!.Auditorium)
                                           .ThenInclude(a => a!.Theater)
                                           .ThenInclude(t => t!.Auditoriums)
                                           .ThenInclude(a => a.Tiers)
                                           .ThenInclude(t => t.Seats)
                                           .SingleOrDefaultAsync(t => t.Id == tierId);

        return mapper.Map<Tier>(infraTier);
    }
}
