using AutoMapper;

namespace CinemaTicketBooking.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Create mappings between infrastructure and domain entities
        CreateMap<Infrastructure.Entities.MovieEntity, Domain.Entities.Movie>()
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.DurationInSeconds)))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name)));

        CreateMap<Domain.Entities.Movie, Infrastructure.Entities.MovieEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)))
            .ForMember(dest => dest.DurationInSeconds, opt => opt.MapFrom(src => src.Duration.Seconds))
            .ForMember(dest => dest.Genres, opt => opt.Ignore());

        CreateMap<Domain.Entities.Theater, Infrastructure.Entities.TheaterEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)));

        CreateMap<Domain.Entities.Auditorium, Infrastructure.Entities.AuditoriumEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)));

        CreateMap<Domain.Entities.Tier, Infrastructure.Entities.TierEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)));

        CreateMap<Domain.Entities.Seat, Infrastructure.Entities.SeatEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)));
    }

    private static Guid CreateNewGuidIfNullOrEmpty(Guid? id)
    {
        if (id is null)
            return Guid.NewGuid();

        return (id.Value == Guid.Empty) ? Guid.NewGuid() : id.Value;
    }
}
