using CinemaTicketBooking.Application.Interfaces.UseCases;

namespace CinemaTicketBooking.Web.Services;

internal class BookingTimeoutBackgroundService : IHostedService, IDisposable
{
    public const double TimeoutLimitInMinutes = 15.0;
    private readonly ILogger<BookingTimeoutBackgroundService> logger;
    private System.Timers.Timer timer;

    public BookingTimeoutBackgroundService(ILogger<BookingTimeoutBackgroundService> logger)
    {

        this.logger = logger;

        timer = new System.Timers.Timer(TimeSpan.FromMinutes(TimeoutLimitInMinutes));
        timer.Elapsed += async (sender, args) => await DoWorkAsync();
        timer.AutoReset = true;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Booking Timeout Background Service is starting.");
        timer.Enabled = true;
        return Task.CompletedTask;
    }

    private async Task DoWorkAsync()
    {
        logger.LogInformation("Booking Timeout Background Service is working.");
        await manageBookingTimeoutUseCase.ExecuteAsync();
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Booking Timeout Background Service is stopping.");
        timer.Enabled = false;
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        timer.Dispose();
    }
}
