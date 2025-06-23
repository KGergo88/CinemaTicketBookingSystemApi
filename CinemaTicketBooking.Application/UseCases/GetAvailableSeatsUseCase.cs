using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.Repositories.Exceptions;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;
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
            var allSeats = await screeningRepository.GetAllSeatsOfTheScreeningAsync(screeningId);
            var reservedSeats = await seatReservationRepository.GetReservedSeatsOfTheScreeningAsync(screeningId);

            var reservedSeatsDictionary = reservedSeats.ToDictionary(rs => rs.Id);
            var availableSeats = allSeats.Where(seat => !reservedSeatsDictionary.ContainsKey(seat.Id))
                                         .ToList();

            return availableSeats;
        }
        catch (Interfaces.Repositories.Exceptions.NotFoundException ex)
        {
            throw new Interfaces.UseCases.Exceptions.NotFoundException($"Could not get available seats for screening {screeningId}. Details: {ex.Message}", ex);
        }
        catch (RepositoryException ex)
        {
            throw new UseCaseException($"Could not get available seats. Details: {ex.Message}", ex);
        }
    }
}
