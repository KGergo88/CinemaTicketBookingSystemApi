using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

internal class AddTheaterUseCase : IAddTheaterUseCase
{
    private readonly ITheaterRepository theaterRepository;

    public AddTheaterUseCase(ITheaterRepository theaterRepository)
    {
        this.theaterRepository = theaterRepository ?? throw new ArgumentNullException(nameof(theaterRepository));
    }

    public async Task ExecuteAsync(Theater theater)
    {
        var existingTheater = await theaterRepository.GetTheaterOrNullAsync(theater.Name, theater.Address);
        if (existingTheater is not null)
            throw new ConflictException("A theater with this name and address already exists! Existing theater:"
                                        + $" ID: {existingTheater.Id}"
                                        + $", Name: {existingTheater.Name}"
                                        + $", Address: {existingTheater.Address}");

        await theaterRepository.AddTheatersAsync([theater]);
    }
}
