using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

internal class GetBookingDetailsUseCase : IGetBookingDetailsUseCase
{
    private readonly IBookingRepository bookingRepository;
    private readonly ITheaterRepository theaterRepository;
    private readonly IScreeningRepository screeningRepository;
    private readonly ISeatReservationRepository seatReservationRepository;

    public GetBookingDetailsUseCase(IBookingRepository bookingRepository,
                                    ITheaterRepository theaterRepository,
                                    IScreeningRepository screeningRepository,
                                    ISeatReservationRepository seatReservationRepository)
    {
        this.bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
        this.theaterRepository = theaterRepository ?? throw new ArgumentNullException(nameof(theaterRepository));
        this.screeningRepository = screeningRepository ?? throw new ArgumentNullException(nameof(screeningRepository));
        this.seatReservationRepository = seatReservationRepository ?? throw new ArgumentNullException(nameof(seatReservationRepository));
    }

    public async Task<BookingDetails> ExecuteAsync(Guid bookingId)
    {
        var booking = await bookingRepository.GetBookingAsync(bookingId);
        if (booking == null)
            throw new GetBookingDetailsException($"Booking not found! Id: {bookingId}");


        var seatReservations = await seatReservationRepository.GetSeatReservationsOfABookingAsync(bookingId);
        if (seatReservations.Count == 0)
            throw new GetBookingDetailsException($"There are no seat reservations for this booking! BookingId: {bookingId}");

        var screeningId = booking.ScreeningId;
        var screening = await screeningRepository.GetScreeningOrNullAsync(screeningId);
        if (screening is null)
            throw new GetBookingDetailsException($"The screening of the booking does not exist! ScreeningId: {screeningId} BookingId: {bookingId}");

        var theater = await theaterRepository.GetTheaterOfAScreeningAsync(screeningId);
        var totalPrice = seatReservations.Sum(sr => sr.Price.Amount);
        var currency = seatReservations.First().Price.Currency;

        var bookingDetails = new BookingDetails()
        {
            Booking = booking,
            Theater = theater,
            Screening = screening,
            SeatReservations = seatReservations,
            TotalPrice = totalPrice,
            Currency = currency,
            CreatedOn = DateTimeOffset.UtcNow
        };

        return bookingDetails;
    }
}
