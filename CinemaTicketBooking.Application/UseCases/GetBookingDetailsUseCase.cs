using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

internal class GetBookingDetailsUseCase : IGetBookingDetailsUseCase
{
    private readonly IBookingRepository bookingRepository;
    private readonly ICustomerRepository customerRepository;
    private readonly IMovieRepository movieRepository;
    private readonly ITheaterRepository theaterRepository;
    private readonly IScreeningRepository screeningRepository;
    private readonly ISeatReservationRepository seatReservationRepository;

    public GetBookingDetailsUseCase(IBookingRepository bookingRepository,
                                    ICustomerRepository customerRepository,
                                    IMovieRepository movieRepository,
                                    ITheaterRepository theaterRepository,
                                    IScreeningRepository screeningRepository,
                                    ISeatReservationRepository seatReservationRepository)
    {
        this.bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
        this.customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        this.movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
        this.theaterRepository = theaterRepository ?? throw new ArgumentNullException(nameof(theaterRepository));
        this.screeningRepository = screeningRepository ?? throw new ArgumentNullException(nameof(screeningRepository));
        this.seatReservationRepository = seatReservationRepository ?? throw new ArgumentNullException(nameof(seatReservationRepository));
    }

    public async Task<BookingDetails> ExecuteAsync(Guid bookingId)
    {
        var booking = await bookingRepository.GetBookingOrNullAsync(bookingId);
        if (booking == null)
            throw new NotFoundException($"Booking not found! Id: {bookingId}");

        var customer = await customerRepository.GetCustomerOrNullAsync(booking.CustomerId);
        if (customer == null)
            throw new NotFoundException($"Customer not found! Id: {booking.CustomerId} BookingId: {bookingId}");

        var screeningId = booking.ScreeningId;
        var screening = await screeningRepository.GetScreeningOrNullAsync(screeningId);
        if (screening is null)
            throw new NotFoundException($"The screening of the booking does not exist! ScreeningId: {screeningId} BookingId: {bookingId}");

        var movieId = screening.MovieId;
        var movie = await movieRepository.GetMovieOrNullAsync(movieId);
        if (movie is null)
            throw new NotFoundException($"The movie of the booking does not exist! Movieid: {movieId} BookingId: {bookingId}");

        var theater = await theaterRepository.GetTheaterOfAScreeningAsync(screeningId);
        var seatReservations = await seatReservationRepository.GetSeatReservationsOfABookingAsync(bookingId);
        theater = FilterNonReservedEntities(theater, seatReservations);

        var totalPriceAmount = seatReservations.Sum(sr => sr.Price.Amount);

        var seatReservationCurrencies = seatReservations.Select(sr => sr.Price.Currency)
                                                        .Distinct();
        // Seat reservation shall have a single currency, except if the total price is zero
        if (seatReservationCurrencies.Count() != 1 && totalPriceAmount > 0)
        {
            var seatReservationCurrenciesAsString = string.Join(", ", seatReservationCurrencies);
            throw new UseCaseException("The currency of the booking is ambiguous!" +
                                       $"Currencies found in the seat reservations of this booking: {seatReservationCurrenciesAsString}");
        }

        var totalPriceCurrency = seatReservationCurrencies.Any() ? seatReservationCurrencies.First() : "";
        var totalPrice = new Price { Amount =  totalPriceAmount, Currency = totalPriceCurrency };

        var bookingDetails = new BookingDetails()
        {
            Booking = booking,
            Customer = customer,
            Theater = theater,
            Screening = screening,
            Movie = movie,
            SeatReservations = seatReservations,
            TotalPrice = totalPrice
        };

        return bookingDetails;
    }

    private Theater FilterNonReservedEntities(Theater theater, List<SeatReservation> seatReservations)
    {
        if (seatReservations.Count == 0)
            return theater;

        var seatReservationsById = seatReservations.Select(sr => sr.SeatId)
                                                   .ToHashSet();

        theater.Auditoriums.ForEach(a => a.Tiers.ForEach(t => t.Seats.FindAll(s => !seatReservationsById.Contains(s.Id))
                                                                     .ForEach(s => t.Seats.Remove(s))));

        theater.Auditoriums.ForEach(a => a.Tiers.RemoveAll(t => t.Seats.Count == 0));

        theater.Auditoriums.RemoveAll(a => a.Tiers.Count == 0);

        return theater;
    }
}
