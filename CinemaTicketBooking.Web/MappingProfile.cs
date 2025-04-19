using AutoMapper;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Web.Dtos;
using CinemaTicketBooking.Web.Dtos.Auditorium;
using CinemaTicketBooking.Web.Dtos.GetAvailableSeats;
using CinemaTicketBooking.Web.Dtos.MakeBooking;
using CinemaTicketBooking.Web.Dtos.Movie;
using CinemaTicketBooking.Web.Dtos.Seat;
using CinemaTicketBooking.Web.Dtos.Tier;
using CinemaTicketBooking.Web.Dtos.Theater;

namespace CinemaTicketBooking.Web;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region "Movie DTOs"
        
        CreateMap<MovieDtoBase, Movie>()
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.DurationInSeconds)));

        CreateMap<MovieWithoutIdDto, Movie>()
            .IncludeBase<MovieDtoBase, Movie>();

        CreateMap<MovieDto, Movie>()
            .IncludeBase<MovieDtoBase, Movie>();

        CreateMap<Movie, MovieDtoBase>()
            .ForMember(dest => dest.DurationInSeconds, opt => opt.MapFrom(src => (int)src.Duration.TotalSeconds));

        CreateMap<Movie, MovieWithoutIdDto>()
            .IncludeBase<Movie, MovieDtoBase>();

        CreateMap<Movie, MovieDto>()
            .IncludeBase<Movie, MovieDtoBase>();

        #endregion

        #region "Seat DTOs"

        CreateMap<SeatDtoBase, Seat>();

        CreateMap<SeatWithoutIdDto, Seat>()
            .IncludeBase<SeatDtoBase, Seat>();

        CreateMap<SeatDto, Seat>()
            .IncludeBase<SeatDtoBase, Seat>();

        CreateMap<Seat, SeatDtoBase>();

        CreateMap<Seat, SeatWithoutIdDto>()
            .IncludeBase<Seat, SeatDtoBase>();

        CreateMap<Seat, SeatDto>()
            .IncludeBase<Seat, SeatDtoBase>();

        #endregion

        #region "Tier DTOs"


        CreateMap<TierWithoutIdDto, Tier>();
        
        CreateMap<TierDto, Tier>();

        CreateMap<Tier, TierWithoutIdDto>();
        
        CreateMap<Tier, TierDto>();

        #endregion

        #region "Auditorium DTOs"

        CreateMap<AuditoriumWithoutIdDto, Auditorium>();

        CreateMap<AuditoriumDto, Auditorium>();

        CreateMap<Auditorium, AuditoriumWithoutIdDto>();

        CreateMap<Auditorium, AuditoriumDto>();

        #endregion


        #region "Theater DTOs"

        CreateMap<TheaterWithoutIdDto, Theater>();
    
        CreateMap<TheaterDto, Theater>();

        CreateMap<Theater, TheaterDto>();


        CreateMap<Theater, TheaterDto>();

        #endregion

        CreateMap<IEnumerable<Seat>, GetAvailableSeatsResponseDto>()
            .ForMember(dest => dest.AvailableSeats, opt => opt.MapFrom(src => src));

        CreateMap<ScreeningDto, Screening>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<Screening, ScreeningDto>();

        CreateMap<PriceDto, Price>();

        CreateMap<Price, PriceDto>();

        CreateMap<PricingDto, Pricing>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<Booking, BookingResponseDto>()
            .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.BookingState, opt => opt.MapFrom(src => (int)src.BookingState));

        CreateMap<Booking, BookingDto>();

        CreateMap<BookingDetails, BookingDetailsDto>();

        CreateMap<SeatReservation, SeatReservationDto>();

        CreateMap<Customer, CustomerDto>();
    }
}
