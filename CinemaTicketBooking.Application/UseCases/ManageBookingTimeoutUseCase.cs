using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.Repositories.Exceptions;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;

namespace CinemaTicketBooking.Application.UseCases;

internal class ManageBookingTimeoutUseCase : IManageBookingTimeoutUseCase
{
    private readonly IBookingRepository bookingRepository;
    private readonly ISeatReservationRepository seatReservationRepository;
    private readonly int ConfirmationTimeoutInMinutes = 15;

    public ManageBookingTimeoutUseCase(IBookingRepository bookingRepository,
                                       ISeatReservationRepository seatReservationRepository)
    {
        this.bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
        this.seatReservationRepository = seatReservationRepository ?? throw new ArgumentNullException(nameof(seatReservationRepository));
    }

    public async Task ExecuteAsync()
    {
        try
        {
            await bookingRepository.TimeoutUnconfirmedBookingsAsync(ConfirmationTimeoutInMinutes);
            await seatReservationRepository.ReleaseSeatsOfTimeoutedBookingsAsync();
        }
        catch (RepositoryException ex)
        {
            throw new UseCaseException($"{nameof(ManageBookingTimeoutUseCase)} failed!", ex);
        }
    }
}
