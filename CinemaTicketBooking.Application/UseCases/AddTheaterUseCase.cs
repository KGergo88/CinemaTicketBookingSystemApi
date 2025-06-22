using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
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
        await theaterRepository.AddTheatersAsync([theater]);
    }
}
