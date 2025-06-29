using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace CinemaTicketBooking.Web;

public class CinemaTicketBookingExceptionHandler : IExceptionHandler
{
    private readonly ILogger<CinemaTicketBookingExceptionHandler> logger;

    public CinemaTicketBookingExceptionHandler(ILogger<CinemaTicketBookingExceptionHandler> logger)
    {
        this.logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError("An unexpected error occurred. Exception: {exception}", exception);

        // Let the default handler deal with it
        return false;
    }
}
