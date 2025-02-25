using AutoMapper;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Web.Dtos;

namespace CinemaTicketBooking.Web;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Create mappings between domain models and DTOs
        CreateMap<Movie, MovieDto>()
            .ForMember(dest => dest.DurationInSeconds, opt => opt.MapFrom(src => (int)src.Duration.TotalSeconds));

        // Additional mappings can be added here
    }
}
