using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Infrastructure.Entities;

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

    public async Task AddTheatersAsync(List<Domain.Entities.Theater> domainTheaters)
    {
        var infraTheaters = mapper.Map<IList<TheaterEntity>>(domainTheaters);
        context.Theaters.AddRange(infraTheaters);
        await context.SaveChangesAsync();
    }
}
