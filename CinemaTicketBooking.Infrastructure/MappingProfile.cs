using AutoMapper;
using Microsoft.IdentityModel.Tokens;

namespace CinemaTicketBooking.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Create mappings between infrastructure and domain entities
        CreateMap<Infrastructure.Entities.Movie, Domain.Entities.Movie>()
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.DurationInSeconds)))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name)));
    }
}
