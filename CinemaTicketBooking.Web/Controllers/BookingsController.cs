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
        try
        {
            var booking = await makeBookingUseCase.ExecuteAsync(
                bookingRequestDto.CustomerId,
                bookingRequestDto.ScreeningId,
                bookingRequestDto.SeatsToReserve);

            var response = mapper.Map<BookingResponseDto>(booking);
            return CreatedAtAction(nameof(GetBookingDetails), new { bookingId = response.BookingId }, response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ConflictException ex)
        {
            return Conflict(ex.Message);
        }
        catch (UseCaseException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{bookingId:guid}/confirm")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ConfirmBooking(Guid bookingId)
    {
        try
        {
            await confirmBookingUseCase.ExecuteAsync(bookingId);
            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ConflictException ex)
        {
            return Conflict(ex.Message);
        }
        catch (UseCaseException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{bookingId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookingDetailsDto>> GetBookingDetails(Guid bookingId)
    {
        try
        {
            var bookingDetails = await getBookingDetailsUseCase.ExecuteAsync(bookingId);
            var response = mapper.Map<BookingDetailsDto>(bookingDetails);
            return Ok(response);
        }
        catch (GetBookingDetailsException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
