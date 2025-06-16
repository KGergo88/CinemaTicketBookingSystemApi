using CinemaTicketBooking.Infrastructure.Entities;

namespace CinemaTicketBooking.Infrastructure.DatabaseSeeding;

internal class SeedData
{
    public List<MovieEntity> Movies { get; set; } = [];

    public List<GenreEntity> Genres { get; set; } = [];

    public List<TheaterEntity> Theaters { get; set; } = [];

    public List<AuditoriumEntity> Auditoriums { get; set; } = [];

    public List<TierEntity> Tiers { get; set; } = [];

    public List<SeatEntity> Seats { get; set; } = [];

    public List<ScreeningEntity> Screenings { get; set; } = [];

    public List<CustomerEntity> Customers { get; set; } = [];

    public List<BookingEntity> Bookings { get; set; } = [];

    public List<SeatReservationEntity> SeatReservations { get; set; } = [];

    public List<PricingEntity> Pricings { get; set; } = [];

    public IEnumerable<object> ToObjects()
    {
        return ToObjects(Movies).Concat(ToObjects(Genres))
                                .Concat(ToObjects(Theaters))
                                .Concat(ToObjects(Auditoriums))
                                .Concat(ToObjects(Tiers))
                                .Concat(ToObjects(Seats))
                                .Concat(ToObjects(Screenings))
                                .Concat(ToObjects(Customers))
                                .Concat(ToObjects(Bookings))
                                .Concat(ToObjects(SeatReservations))
                                .Concat(ToObjects(Pricings));
    }

    private static IEnumerable<object> ToObjects<TEntity>(IEnumerable<TEntity> entities)
    {
        return (entities ?? Enumerable.Empty<TEntity>()).Cast<object>();
    }
}
