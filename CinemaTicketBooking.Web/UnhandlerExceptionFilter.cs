using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBooking.Web;

internal class UnhandledExceptionFilter : IExceptionFilter
{
    private readonly ILogger<UnhandledExceptionFilter> logger;
    private const string BasicErrorMessage = "An unexpected error occurred.";

    public UnhandledExceptionFilter(ILogger<UnhandledExceptionFilter> logger)
    {
        this.logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        // Logging the exception details
        logger.LogError("{basicErrorMessage} Exception: {exception}", BasicErrorMessage, context.Exception);

        // Returning only a basic error message to the client without exposing sensitive information
        context.Result = new JsonResult(BasicErrorMessage)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}
