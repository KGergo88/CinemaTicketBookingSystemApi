using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

internal class GetAvailableSeatsUseCase : IGetAvailableSeatsUseCase
{
    private readonly IScreeningRepository screeningRepository;
    private readonly ISeatReservationRepository seatReservationRepository;

    public GetAvailableSeatsUseCase(IScreeningRepository screeningRepository,
                                    ISeatReservationRepository seatReservationRepository)
    {
        this.screeningRepository = screeningRepository ?? throw new ArgumentNullException(nameof(screeningRepository));
        this.seatReservationRepository = seatReservationRepository ?? throw new ArgumentNullException(nameof(seatReservationRepository));
    }

    public async Task<List<Seat>> ExecuteAsync(Guid screeningId)
    {
        try
        {
            var allSeatsTask = screeningRepository.GetAllSeatsOfTheScreeningAsync(screeningId);
            var reserveSeatsTask = seatReservationRepository.GetReservedSeatsOfTheScreeningAsync(screeningId);
            await Task.WhenAll(allSeatsTask, reserveSeatsTask);
            var allSeats = allSeatsTask.Result;
            var reservedSeats = reserveSeatsTask.Result;

            var reservedSeatsDictionary = reservedSeats.ToDictionary(rs => rs.Id);
            var availableSeats = allSeats.Where(seat => !reservedSeatsDictionary.ContainsKey(seat.Id))
                                         .ToList();

            return availableSeats;
        }
        catch (SeatReservationRepositoryException ex)
        {
            throw new GetAvailableSeatsUseCaseException($"Could not get available seats. Details: {ex.Message}", ex);
        }
    }
}
