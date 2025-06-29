using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

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
        var exceptionHandlerData = new ExceptionHandlerData(httpContext, exception, cancellationToken);

        switch (exception)
        {
            case NotFoundException:
                return await HandleKnownExceptionsAsync(exceptionHandlerData, StatusCodes.Status404NotFound);

            case ConflictException:
                return await HandleKnownExceptionsAsync(exceptionHandlerData, StatusCodes.Status409Conflict);

            case UseCaseException:
                return await HandleKnownExceptionsAsync(exceptionHandlerData, StatusCodes.Status400BadRequest);

            default:
                logger.LogError("An unexpected error occurred. Exception: {exception}", exception);

                // Let the default handler deal with it
                return false;
        }
    }

    private record ExceptionHandlerData(HttpContext HttpContext, Exception Exception, CancellationToken CancellationToken);

    private async ValueTask<bool> HandleKnownExceptionsAsync(ExceptionHandlerData data,
                                                             int statusCodeToSet)
    {
        logger.LogWarning("A known exception occurred during serving the request: {exception}", data.Exception);

        data.HttpContext.Response.ContentType = "application/json";
        data.HttpContext.Response.StatusCode = statusCodeToSet;

        var errorResponse = JsonSerializer.Serialize(new { error = data.Exception.Message });
        await data.HttpContext.Response.WriteAsync(errorResponse, data.CancellationToken);

        return true;
    }
}
