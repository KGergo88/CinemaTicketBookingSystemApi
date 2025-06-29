using Microsoft.AspNetCore.Hosting;

namespace CinemaTicketBooking.IntegrationTests;

internal static class IWebHostBuilderExtensions
{
    public static IWebHostBuilder UseSettings(this IWebHostBuilder builder, IDictionary<string, string?> settings)
    {
        foreach (var (key, value) in settings)
        {
            builder.UseSetting(key, value);
        }
        return builder;
    }
}
