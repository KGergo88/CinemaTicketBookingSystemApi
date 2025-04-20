using CinemaTicketBooking.Infrastructure.Entities;

namespace CinemaTicketBooking.Infrastructure.DatabaseSeeding;

internal class SeedData
{
    public required List<MovieEntity> Movies { get; set; }

    public required List<GenreEntity> Genres { get; set; }

    public required List<TheaterEntity> Theaters { get; set; }

    public required List<AuditoriumEntity> Auditoriums { get; set; }

    public required List<TierEntity> Tiers { get; set; }

    public required List<SeatEntity> Seats { get; set; }

    public required List<ScreeningEntity> Screenings { get; set; }

    public required List<CustomerEntity> Customers { get; set; }

    public required List<BookingEntity> Bookings { get; set; }

    public required List<SeatReservationEntity> SeatReservations { get; set; }

    public required List<PricingEntity> Pricings { get; set; }

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

    private IEnumerable<object> ToObjects<TEntity>(IEnumerable<TEntity> entities)
    {
        return (entities ?? Enumerable.Empty<TEntity>()).Cast<object>();
    }
}
