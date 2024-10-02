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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
