using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBooking.Web.Controllers;

[Route("[controller]")]
public class HomeController : Controller
{
    [HttpGet("/")]
    [HttpGet("[action]")]
    public IActionResult Index()
    {
        var htmlContent = "<h1>Cinema Ticket Booking System API</h1>";
        return Content(htmlContent, "text/html");
    }

    [HttpGet("[action]")]
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
