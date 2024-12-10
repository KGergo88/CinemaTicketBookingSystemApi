using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Infrastructure.Entities;

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

    public async Task AddScreeningsAsync(List<Domain.Entities.Screening> domainScreenings)
    {
        var infraScreenings = mapper.Map<IList<ScreeningEntity>>(domainScreenings);
        context.Screenings.AddRange(infraScreenings);
        await context.SaveChangesAsync();
    }
}
