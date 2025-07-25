using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.Repositories.Exceptions;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;
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

    public async Task<Booking> ExecuteAsync(Guid customerId, Guid screeningId, IEnumerable<Guid> seatIdsToReserve)
    {
        var customer = await customerRepository.GetCustomerOrNullAsync(customerId);
        if (customer is null)
            throw new Interfaces.UseCases.Exceptions.NotFoundException($"Unknown customer Id: {customerId}");

        var screening = await screeningRepository.GetScreeningOrNullAsync(screeningId);
        if (screening is null)
            throw new Interfaces.UseCases.Exceptions.NotFoundException($"Unknown screening Id: {screeningId}");

        if (screening.Showtime < DateTime.UtcNow)
            throw new UseCaseException($"The screening's showtime is in the past, thus not bookable.");

        if (!seatIdsToReserve.Any())
            throw new UseCaseException($"Cannot make a booking without reserving any seats!");

        var notExistingSeatIds = await screeningRepository.FindNotExistingSeatIdsAsync(screeningId, seatIdsToReserve);
        if (notExistingSeatIds.Any())
        {
            var notExistingSeatIdsAsString = string.Join(", ", notExistingSeatIds.Select(nes => nes));
            throw new Interfaces.UseCases.Exceptions.NotFoundException($"Could not reserve seats as the following seats do not exist: {notExistingSeatIdsAsString}");
        }

        var alreadyReservedSeatIds = await seatReservationRepository.FindAlreadyReservedSeatIdsAsync(screeningId, seatIdsToReserve);
        if (alreadyReservedSeatIds.Any())
        {
            var alreadyReservedSeatIdsAsString = string.Join(", ", alreadyReservedSeatIds);
            throw new ConflictException($"Could not reserve seats as the following seats are already reserved: {alreadyReservedSeatIdsAsString}");
        }

        var pricingsBySeatId = await screeningRepository.GetPricingsBySeatIdAsync(screeningId);
        if (seatIdsToReserve.Any(sitr => !pricingsBySeatId.ContainsKey(sitr)))
            throw new Interfaces.UseCases.Exceptions.NotFoundException("Could not reserve seats as pricing information is not available for at least one of them.");

        Booking booking;
        try
        {
            booking = new Booking()
            {
                Id = Guid.NewGuid(),
                BookingState = BookingState.NonConfirmed,
                CustomerId = customer.Id,
                ScreeningId = screeningId,
                CreatedOn = DateTimeOffset.UtcNow
            };
            await bookingRepository.AddBookingAsync(booking);
        }
        catch
        {
            throw new UseCaseException($"Could not create booking! No seats were reserved!");
        }

        try
        {
            var seatReservations = seatIdsToReserve.Select(sitr => new SeatReservation()
            {
                BookingId = booking.Id,
                ScreeningId = screeningId,
                SeatId = sitr,
                Price = pricingsBySeatId[sitr].Price,
            }).ToList();

            await seatReservationRepository.AddSeatReservationsAsync(seatReservations);
        }
        catch (RepositoryException exception)
        {
            await bookingRepository.DeleteBookingAsync(booking.Id);

            if (exception is DuplicateException)
            {
                throw new ConflictException($"Some of the seats are already reserved! Error: \"{exception.Message}\"", exception);
            }
            else
            {
                throw new UseCaseException($"Could not reserve seats! Error: \"{exception.Message}\"", exception);
            }
        }

        return booking;
    }
}
