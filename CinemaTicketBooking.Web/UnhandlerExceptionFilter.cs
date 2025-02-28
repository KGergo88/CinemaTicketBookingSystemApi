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
        logger.LogError("{basicErrorMessage} Exception: {exception}", BasicErrorMessage, context.Exception);

        context.Result = new JsonResult(BasicErrorMessage)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}
