using CinemaTicketBooking.Application.Interfaces.UseCases;

namespace CinemaTicketBooking.Web.Services;

internal class BookingTimeoutBackgroundService : IHostedService, IDisposable
{
    public const double TimerPeriodInMinutes = 15.0;
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly ILogger<BookingTimeoutBackgroundService> logger;
    private System.Timers.Timer timer;

    public BookingTimeoutBackgroundService(IServiceScopeFactory serviceScopeFactory,
                                           ILogger<BookingTimeoutBackgroundService> logger)
    {
        this.serviceScopeFactory = serviceScopeFactory;

        this.logger = logger;

        timer = new System.Timers.Timer(TimeSpan.FromMinutes(TimerPeriodInMinutes));
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
        using var scope = serviceScopeFactory.CreateScope();
        var manageBookingTimeoutUseCase = scope.ServiceProvider.GetRequiredService<IManageBookingTimeoutUseCase>();
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
