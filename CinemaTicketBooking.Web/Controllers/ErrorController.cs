using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBooking.Web.Controllers;

public class ErrorController : Controller
{
    [Route("/error")]
    [HttpGet]
    public IActionResult Error()
    {
        var htmlContent = "<h1>Error</h1><p>An error occurred while processing your request.</p>";
        return Content(htmlContent, "text/html");
    }

    public IActionResult HandleUnknownRoutes()
    {
        return NotFound($"No route matched!");
    }
}
