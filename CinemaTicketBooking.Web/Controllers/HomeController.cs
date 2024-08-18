using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBooking.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var htmlContent = "<h1>Cinema Ticket Booking System API</h1>";
        return Content(htmlContent, "text/html");
    }
}
