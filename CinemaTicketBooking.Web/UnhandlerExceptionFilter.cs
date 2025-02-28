using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBooking.Web;

internal class UnhandledExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var errorResponse = new
        {
            Message = "An unexpected error occurred.",
            // We may not want to show the details to the client in a production environment
            // as it may cause vulnerabilities or contain secrets
            Detail = context.Exception.Message
        };

        context.Result = new JsonResult(errorResponse)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}