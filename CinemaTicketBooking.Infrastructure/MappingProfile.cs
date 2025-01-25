using AutoMapper;

namespace CinemaTicketBooking.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Infrastructure.Entities.MovieEntity, Domain.Entities.Movie>()
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.DurationInSeconds)))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name)));

        CreateMap<Domain.Entities.Movie, Infrastructure.Entities.MovieEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)))
            .ForMember(dest => dest.DurationInSeconds, opt => opt.MapFrom(src => src.Duration.Seconds))
            .ForMember(dest => dest.Genres, opt => opt.Ignore());

        CreateMap<Infrastructure.Entities.ScreeningEntity, Domain.Entities.Screening>();

        // When storing domain entities we only need to fill out the XXXId properties for Auditorium and Movie, the navigation properties are irrelevant
        CreateMap<Domain.Entities.Screening, Infrastructure.Entities.ScreeningEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)))
            .ForMember(dest => dest.Auditorium, opt => opt.Ignore())
            .ForMember(dest => dest.Movie, opt => opt.Ignore());

        CreateMap<Domain.Entities.Theater, Infrastructure.Entities.TheaterEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)));

        CreateMap<Infrastructure.Entities.AuditoriumEntity, Domain.Entities.Auditorium>();

        CreateMap<Domain.Entities.Auditorium, Infrastructure.Entities.AuditoriumEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)));

        CreateMap<Infrastructure.Entities.TierEntity, Domain.Entities.Tier>();

        CreateMap<Domain.Entities.Tier, Infrastructure.Entities.TierEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)));

        CreateMap<Infrastructure.Entities.SeatEntity, Domain.Entities.Seat>();

        CreateMap<Domain.Entities.Seat, Infrastructure.Entities.SeatEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)));

        CreateMap<Infrastructure.Entities.CustomerEntity, Domain.Entities.Customer>();

        // When storing domain entities we only need to fill out the XXXId properties, the navigation properties are irrelevant
        CreateMap<Domain.Entities.Booking, Infrastructure.Entities.BookingEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)))
            .ForMember(dest => dest.BookingState, opt => opt.MapFrom(src => (int)src.BookingState))
            .ForMember(dest => dest.Customer, opt => opt.Ignore());

        CreateMap<Infrastructure.Entities.BookingEntity, Domain.Entities.Booking>()
            .ForMember(dest => dest.BookingState, opt => opt.MapFrom(src => (Domain.Entities.BookingState)src.BookingState));

        CreateMap<Domain.Entities.Customer, Infrastructure.Entities.CustomerEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)));

        CreateMap<Domain.Entities.Pricing, Infrastructure.Entities.PricingEntity>()
            // For previously not stored entities (Guid is empty), a valid Guid needs to be created
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CreateNewGuidIfNullOrEmpty(src.Id)))
            .ForMember(dest => dest.Screening, opt => opt.Ignore())
            .ForMember(dest => dest.Tier, opt => opt.Ignore())
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency));

        CreateMap<Infrastructure.Entities.SeatReservationEntity, Domain.Entities.SeatReservation>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => new Domain.Entities.Price{ Amount = src.Price, Currency = src.Currency }));
    }

    private static Guid CreateNewGuidIfNullOrEmpty(Guid? id)
    {
        if (id is null)
            return Guid.NewGuid();

        return (id.Value == Guid.Empty) ? Guid.NewGuid() : id.Value;
    }
}
