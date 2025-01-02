using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.UseCases;
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
    private readonly IConfirmBookingUseCase confirmBookingUseCase;

    public BookingController(
        IMapper mapper,
        IMakeBookingUseCase makeBookingUseCase,
        IConfirmBookingUseCase confirmBookingUseCase)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.makeBookingUseCase = makeBookingUseCase ?? throw new ArgumentNullException(nameof(makeBookingUseCase));
        this.confirmBookingUseCase = confirmBookingUseCase ?? throw new ArgumentNullException(nameof(confirmBookingUseCase));
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

    [HttpPost("[action]")]
    public async Task<ActionResult> ConfirmBooking(Guid bookingId)
    {
        try
        {
            await confirmBookingUseCase.ExecuteAsync(bookingId);
            return Ok();
        }
        catch (ConfirmBookingException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
