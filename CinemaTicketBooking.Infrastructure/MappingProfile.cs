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
            .ForMember(dest => dest.DurationInSeconds, opt => opt.MapFrom(src => src.Duration.Seconds))
            .ForMember(dest => dest.Genres, opt => opt.Ignore());
    }
}
