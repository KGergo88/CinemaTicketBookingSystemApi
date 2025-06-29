using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;
using CinemaTicketBooking.Web.Dtos;
using CinemaTicketBooking.Web.Dtos.MakeBooking;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBooking.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IMakeBookingUseCase makeBookingUseCase;
    private readonly IConfirmBookingUseCase confirmBookingUseCase;
    private readonly IGetBookingDetailsUseCase getBookingDetailsUseCase;

    public BookingsController(
        IMapper mapper,
        IMakeBookingUseCase makeBookingUseCase,
        IConfirmBookingUseCase confirmBookingUseCase,
        IGetBookingDetailsUseCase getBookingDetailsUseCase)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.makeBookingUseCase = makeBookingUseCase ?? throw new ArgumentNullException(nameof(makeBookingUseCase));
        this.confirmBookingUseCase = confirmBookingUseCase ?? throw new ArgumentNullException(nameof(confirmBookingUseCase));
        this.getBookingDetailsUseCase = getBookingDetailsUseCase ?? throw new ArgumentNullException(nameof(getBookingDetailsUseCase));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookingResponseDto>> MakeBooking([FromBody] BookingRequestDto bookingRequestDto)
    {
        var booking = await makeBookingUseCase.ExecuteAsync(
            bookingRequestDto.CustomerId,
            bookingRequestDto.ScreeningId,
            bookingRequestDto.SeatsToReserve);

        var response = mapper.Map<BookingResponseDto>(booking);
        return CreatedAtAction(nameof(GetBookingDetails), new { bookingId = response.BookingId }, response);
    }

    [HttpPost("{bookingId:guid}/confirm")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ConfirmBooking(Guid bookingId)
    {
        await confirmBookingUseCase.ExecuteAsync(bookingId);
        return Ok();
    }

    [HttpGet("{bookingId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookingDetailsDto>> GetBookingDetails(Guid bookingId)
    {
        var bookingDetails = await getBookingDetailsUseCase.ExecuteAsync(bookingId);
        var response = mapper.Map<BookingDetailsDto>(bookingDetails);
        return Ok(response);
    }
}
