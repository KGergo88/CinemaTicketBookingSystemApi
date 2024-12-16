using AutoMapper;
using CinemaTicketBooking.Application.UseCases;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Web.Dtos.MakeBooking;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBooking.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IMakeBookingUseCase makeBookingUseCase;

    public BookingController(
        IMapper mapper,
        IMakeBookingUseCase makeBookingUseCase)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.makeBookingUseCase = makeBookingUseCase ?? throw new ArgumentNullException(nameof(makeBookingUseCase));
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> MakeBooking(Dtos.MakeBooking.BookingRequestDto bookingRequestDto)
    {
        try
        {
            var booking = await makeBookingUseCase.ExecuteAsync(
                bookingRequestDto.CustomerId,
                bookingRequestDto.ScreeningId,
                bookingRequestDto.SeatsToReserve);

            var response = mapper.Map<BookingResponseDto>(booking);
            return Ok(response);
        }
        catch (MakeBookingException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
