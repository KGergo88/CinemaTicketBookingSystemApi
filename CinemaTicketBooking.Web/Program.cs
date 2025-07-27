using CinemaTicketBooking.Application;
using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.DatabaseBindings;
using CinemaTicketBooking.Web;
using CinemaTicketBooking.Web.Services;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.Async(a => a.File("CinemaTicketBooking-.log", rollingInterval: RollingInterval.Day))
    .CreateLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilog();

    builder.Services.AddAutoMapper(
        typeof(CinemaTicketBooking.Infrastructure.MappingProfile),
        typeof(CinemaTicketBooking.Web.MappingProfile));

    var connectionStringName = "CinemaTicketBooking";
    var dbConnectionString = builder.Configuration.GetConnectionString(connectionStringName);

    var databaseBinding = DatabaseBindingFactory.Create(dbConnectionString);
    builder.Services.AddDbContext<CinemaTicketBookingDbContext>(
        options => databaseBinding.SetDatabaseType(options, dbConnectionString));

    builder.Services.AddControllers();

    builder.Services.AddCinemaTicketBookingInfrastructureServices()
                    .AddCinemaTicketBookingApplicationServices()
                    .AddHostedService<BookingTimeoutBackgroundService>()
                    .AddExceptionHandler<CinemaTicketBookingExceptionHandler>();

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Cinema Ticket Booking System API",
            Description = "",
            Version = "v1",
            TermsOfService = null,
            Contact = null,
            License = new OpenApiLicense
            {
                Name = "MIT",
                Url = new Uri("https://github.com/KGergo88/CinemaTicketBookingSystemApi/blob/main/license")
            }
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseExceptionHandler("/error");

    app.MapControllers();
    app.MapFallbackToController("HandleUnknownRoutes", "Error");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal("Application terminated unexpectedly!" +
              " If you encounter this when running 'dotnet ef' commands, this is not necessarily an error!" +
              " Exception: {ex}", ex);
}
finally
{
    Log.CloseAndFlush();
}

// This empty partial class is required for the integration tests to work correctly
// as it allows the Program class to be used with WebApplicationFactory<T>.
public partial class Program { }
