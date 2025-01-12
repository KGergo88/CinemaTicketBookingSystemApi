using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

internal class GetBookingDetailsUseCase : IGetBookingDetailsUseCase
{
    private readonly IBookingRepository bookingRepository;
    private readonly ITheaterRepository theaterRepository;
    private readonly ISeatReservationRepository seatReservationRepository;

    public GetBookingDetailsUseCase(IBookingRepository bookingRepository,
                                    ITheaterRepository theaterRepository,
                                    ISeatReservationRepository seatReservationRepository)
    {
        this.bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
        this.theaterRepository = theaterRepository ?? throw new ArgumentNullException(nameof(theaterRepository));
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

        var screening = seatReservations.FirstOrDefault()?.Screening;
        if (screening?.Id is null || screening.Id == Guid.Empty)
            throw new GetBookingDetailsException($"Could not get screening data from seat reservations! BookingId: {bookingId}");

        var theater = await theaterRepository.GetTheaterOfAScreeningAsync(screening.Id.Value);
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
