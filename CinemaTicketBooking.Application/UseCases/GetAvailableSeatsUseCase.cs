using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

internal class GetAvailableSeatsUseCase : IGetAvailableSeatsUseCase
{
    private readonly ISeatReservationRepository seatReservationRepository;

    public GetAvailableSeatsUseCase(ISeatReservationRepository seatReservationRepository)
    {
        this.seatReservationRepository = seatReservationRepository ?? throw new ArgumentNullException(nameof(seatReservationRepository));
    }

    public async Task<List<Seat>> ExecuteAsync(Guid screeningId)
    {
        try
        {
           return await seatReservationRepository.GetAvailableSeatsAsync(screeningId);
        }
        catch (SeatReservationRepositoryException ex)
        {
            throw new GetAvailableSeatsUseCaseException($"Could not get available seats. Details: {ex.Message}", ex);
        }
    }
}
