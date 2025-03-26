using AutoMapper;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Infrastructure.Entities;

namespace CinemaTicketBooking.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<MovieEntity, Movie>()
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.DurationInSeconds)))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name)));

        CreateMap<Movie, MovieEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)))
            .ForMember(dest => dest.DurationInSeconds, opt => opt.MapFrom(src => src.Duration.TotalSeconds))
            .ForMember(dest => dest.Genres, opt => opt.Ignore());

        CreateMap<ScreeningEntity, Screening>();

        // When storing domain entities we only need to fill out the XXXId properties for Auditorium and Movie, the navigation properties are irrelevant
        CreateMap<Screening, ScreeningEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)))
            .ForMember(dest => dest.Auditorium, opt => opt.Ignore())
            .ForMember(dest => dest.Movie, opt => opt.Ignore());

        CreateMap<Theater, TheaterEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)));

        CreateMap<AuditoriumEntity, Auditorium>();

        CreateMap<Auditorium, AuditoriumEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)));

        CreateMap<TierEntity, Tier>();

        CreateMap<Tier, TierEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)));

        CreateMap<SeatEntity, Seat>();

        CreateMap<Seat, SeatEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)));

        CreateMap<CustomerEntity, Customer>();

        // When storing domain entities we only need to fill out the XXXId properties, the navigation properties are irrelevant
        CreateMap<Booking, BookingEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)))
            .ForMember(dest => dest.BookingState, opt => opt.MapFrom(src => (int)src.BookingState))
            .ForMember(dest => dest.Customer, opt => opt.Ignore());

        CreateMap<BookingEntity, Booking>()
            .ForMember(dest => dest.BookingState, opt => opt.MapFrom(src => (BookingState)src.BookingState));

        CreateMap<Customer, CustomerEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)));

        CreateMap<Pricing, PricingEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)))
            .ForMember(dest => dest.Screening, opt => opt.Ignore())
            .ForMember(dest => dest.Tier, opt => opt.Ignore())
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency));

        CreateMap<SeatReservationEntity, SeatReservation>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => new Price{ Amount = src.Price, Currency = src.Currency }));

        CreateMap<SeatReservation, SeatReservationEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency));
    }

    private static Guid CreateNewGuidIfNullOrEmpty(Guid? id)
    {
        if (id is null)
            return Guid.NewGuid();

        return (id.Value == Guid.Empty) ? Guid.NewGuid() : id.Value;
    }
}
