﻿using Microsoft.AspNetCore.Mvc;

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
}
