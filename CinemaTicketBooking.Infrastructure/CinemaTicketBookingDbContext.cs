using CinemaTicketBooking.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.Infrastructure;

public class CinemaTicketBookingDbContext : DbContext
{
    public CinemaTicketBookingDbContext(DbContextOptions<CinemaTicketBookingDbContext> options)
        : base(options)
    {
    }

    internal DbSet<MovieEntity> Movies { get; set; }

    internal DbSet<GenreEntity> Genres { get; set; }

    internal DbSet<TheaterEntity> Theaters { get; set; }

    internal DbSet<AuditoriumEntity> Auditoriums { get; set; }

    internal DbSet<TierEntity> Tiers { get; set; }

    internal DbSet<SeatEntity> Seats { get; set; }

    internal DbSet<ScreeningEntity> Screenings { get; set; }

    internal DbSet<CustomerEntity> Customers { get; set; }

    internal DbSet<BookingEntity> Bookings { get; set; }

    internal DbSet<SeatReservationEntity> SeatReservations { get; set; }

    internal DbSet<PricingEntity> Pricings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Defining deletion behaviours for SeatReservation in order to solve database creation issue.
        // The Auditoriums table can be reached from the SeatReservations table in two routes.
        // This leads to the problem that deleting the parent in these relationships cannot delete the children.
        // Since deleting is not a case we want to solve now,
        // we can define the NoAction behaviour which will cause exceptions to be thrown upon deleting.
        // Link:https://learn.microsoft.com/en-us/ef/core/saving/cascade-delete#configuring-cascading-behaviors
        modelBuilder
            .Entity<SeatReservationEntity>()
            .HasOne(e => e.Screening)
            .WithMany(e => e.SeatReservations)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder
            .Entity<SeatReservationEntity>()
            .HasOne(e => e.Seat)
            .WithMany(e => e.SeatReservations)
            .OnDelete(DeleteBehavior.NoAction);

        // Defining deletion behaviours for Pricing in order to solve database creation issue.
        // The Auditoriums table can be reached from the Pricings table in two routes.
        // This leads to the problem that deleting the parent in these relationships cannot delete the children.
        // Since deleting is not a case we want to solve now,
        // we can define the NoAction behaviour which will cause exceptions to be thrown upon deleting.
        // Link:https://learn.microsoft.com/en-us/ef/core/saving/cascade-delete#configuring-cascading-behaviors
        modelBuilder
            .Entity<PricingEntity>()
            .HasOne(e => e.Screening)
            .WithMany(e => e.Pricings)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder
            .Entity<PricingEntity>()
            .HasOne(e => e.Tier)
            .WithOne(e => e.Pricing)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
