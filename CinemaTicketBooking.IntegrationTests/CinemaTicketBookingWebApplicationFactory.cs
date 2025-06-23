using CinemaTicketBooking.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

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
}
