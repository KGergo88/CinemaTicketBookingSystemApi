using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

internal class MakeBookingUseCase : IMakeBookingUseCase
{
    private readonly IBookingRepository bookingRepository;
    private readonly ICustomerRepository customerRepository;
    private readonly IScreeningRepository screeningRepository;
    private readonly ISeatReservationRepository seatReservationRepository;

    public MakeBookingUseCase(IBookingRepository bookingRepository,
                              ICustomerRepository customerRepository,
                              IScreeningRepository screeningRepository,
                              ISeatReservationRepository seatReservationRepository)
    {
        this.bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
        this.customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        this.screeningRepository = screeningRepository ?? throw new ArgumentNullException(nameof(screeningRepository));
        this.seatReservationRepository = seatReservationRepository ?? throw new ArgumentNullException(nameof(seatReservationRepository));
    }

    public async Task<Booking> ExecuteAsync(Guid customerId, Guid screeningId, IEnumerable<Guid> seatsToReserve)
    {
        var customer = await customerRepository.GetCustomerOrNullAsync(customerId);
        if (customer is null)
            throw new MakeBookingException($"Unknown customer Id: {customerId}");

        var screening = await screeningRepository.GetScreeningOrNullAsync(screeningId);
        if (screening is null)
            throw new MakeBookingException($"Unknown screening Id: {screeningId}");

        if (screening.Showtime < DateTime.UtcNow)
            throw new MakeBookingException($"The screening's showtime is in the past, thus not bookable.");

        var bookingId = Guid.NewGuid();
        try
        {
            await seatReservationRepository.AddSeatReservationsAsync(seatsToReserve, bookingId, screeningId);
        }
        catch (SeatReservationRepositoryException exception)
        {
            throw new MakeBookingException($"Could not reserve seats! Error: \"{exception.Message}\"", exception);
        }

        var booking = new Booking()
        {
            Id = bookingId,
            BookingState = BookingState.NonConfirmed,
            Customer = customer,
            CreatedOn = DateTime.UtcNow
        };
        await bookingRepository.AddBookingAsync(booking);

        return booking;
    }
}
