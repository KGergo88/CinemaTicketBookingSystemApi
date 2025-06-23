using CinemaTicketBooking.Application;
using CinemaTicketBooking.Infrastructure;
using CinemaTicketBooking.Infrastructure.DatabaseBindings;
using CinemaTicketBooking.Web;
using CinemaTicketBooking.Web.Services;
using Microsoft.EntityFrameworkCore;
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
    if (dbConnectionString is null)
        throw new StartupException($"Could not load the connection string: \"{connectionStringName}\"");

    var databaseBinding = DatabaseBindingFactory.Create(dbConnectionString);
    builder.Services.AddDbContext<CinemaTicketBookingDbContext>(
        options => databaseBinding.SetDatabaseType(options, dbConnectionString));

    builder.Services.AddControllers(options =>
        options.Filters.Add<UnhandledExceptionFilter>());

    builder.Services.AddCinemaTicketBookingInfrastructureServices()
                    .AddCinemaTicketBookingApplicationServices()
                    .AddHostedService<BookingTimeoutBackgroundService>();

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
        app.UseDeveloperExceptionPage();

        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        app.UseExceptionHandler("/home/error");
    }

    app.MapControllers();
    app.MapFallbackToController("HandleUnknownRoutes", "Home");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

// This empty partial class is required for the integration tests to work correctly
// as it allows the Program class to be used with WebApplicationFactory<T>.
public partial class Program { }
