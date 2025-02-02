using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Infrastructure.DatabaseBindings;
using CinemaTicketBooking.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaTicketBooking.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCinemaTicketBookingInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IDatabaseBinding, SqlServerDatabaseBinding>();

        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IScreeningRepository, ScreeningRepository>();
        services.AddScoped<ISeatReservationRepository, SeatReservationRepository>();
        services.AddScoped<ITheaterRepository, TheaterRepository>();

        return services;
    }
}
