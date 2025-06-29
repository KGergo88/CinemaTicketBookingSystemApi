using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaTicketBooking.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCinemaTicketBookingApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAddMovieUseCase, AddMovieUseCase>();
        services.AddScoped<IAddScreeningUseCase, AddScreeningUseCase>();
        services.AddScoped<IAddTheaterUseCase, AddTheaterUseCase>();
        services.AddScoped<IConfirmBookingUseCase, ConfirmBookingUseCase>();
        services.AddScoped<IDeleteMovieUseCase, DeleteMovieUseCase>();
        services.AddScoped<IGetAvailableSeatsUseCase, GetAvailableSeatsUseCase>();
        services.AddScoped<IGetBookingDetailsUseCase, GetBookingDetailsUseCase>();
        services.AddScoped<IGetMoviesUseCase, GetMoviesUseCase>();
        services.AddScoped<IMakeBookingUseCase, MakeBookingUseCase>();
        services.AddScoped<IManageBookingTimeoutUseCase, ManageBookingTimeoutUseCase>();
        services.AddScoped<ISetPricingUseCase, SetPricingUseCase>();
        services.AddScoped<IUpdateMovieUseCase, UpdateMovieUseCase>();

        return services;
    }
}
