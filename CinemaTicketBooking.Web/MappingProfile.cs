using AutoMapper;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Web.Dtos;
using CinemaTicketBooking.Web.Dtos.Movie;

namespace CinemaTicketBooking.Web;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<MovieWithoutIdDto, Movie>()
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.DurationInSeconds)));

        CreateMap<MovieWithIdDto, Movie>()
            .IncludeBase<MovieWithoutIdDto, Movie>();

        CreateMap<Movie, MovieWithoutIdDto>()
            .ForMember(dest => dest.DurationInSeconds, opt => opt.MapFrom(src => (int)src.Duration.TotalSeconds));

        CreateMap<Movie, MovieWithIdDto>()
            .IncludeBase<Movie, MovieWithoutIdDto>();

        CreateMap<TheaterDto, Theater>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<AuditoriumDto, Auditorium>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<TierDto, Tier>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<SeatDto, Seat>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<ScreeningDto, Screening>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
