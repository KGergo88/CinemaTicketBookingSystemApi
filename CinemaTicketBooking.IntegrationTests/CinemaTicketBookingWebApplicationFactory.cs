using CinemaTicketBooking.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaTicketBooking.IntegrationTests;

public class CinemaTicketBookingWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly Action<IServiceCollection>? overrideServicesAction;

    public CinemaTicketBookingWebApplicationFactory(Action<IServiceCollection>? overrideServices = null)
    {
        this.overrideServicesAction = overrideServices;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Removing the existing DbContext created based on the connection string of the configuration
            var dbContextDescriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<CinemaTicketBookingDbContext>));
            services.Remove(dbContextDescriptor);
            // Adding an in-memory database. This is only a placeholder and shall not be used by the tests.
            // See reasoning here: https://learn.microsoft.com/en-us/ef/core/providers/in-memory/?tabs=dotnet-core-cli
            services.AddDbContext<CinemaTicketBookingDbContext>(options =>
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });

            // Apply dependency overrides
            overrideServicesAction?.Invoke(services);
        });
    }

    /// <summary>
    /// Creates a new <see cref="HttpClient"/> instance configured with custom application settings.
    /// This method shall be used to workaround the issue: https://github.com/dotnet/aspnetcore/issues/37680
    /// </summary>
    /// <param name="appSettings">
    /// A dictionary of key-value pairs representing application settings to override the default configuration.
    /// Typically used to inject test-specific configuration values such as connection strings.
    /// </param>
    /// <returns>
    /// A configured <see cref="HttpClient"/> instance for sending HTTP requests to the test server.
    /// </returns>
    public HttpClient CreateClient(IDictionary<string, string?> appSettings)
    {
        var httpClient = WithWebHostBuilder(builder =>
        {
            builder.UseSettings(appSettings);
        }).CreateClient();

        return httpClient;
    }
}
