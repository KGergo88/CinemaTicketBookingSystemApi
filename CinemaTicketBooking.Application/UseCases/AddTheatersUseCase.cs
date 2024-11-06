using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

internal class AddTheatersUseCase : IAddTheatersUseCase
{
    private readonly ITheaterRepository theaterRepository;

    public AddTheatersUseCase(ITheaterRepository theaterRepository)
    {
        this.theaterRepository = theaterRepository ?? throw new ArgumentNullException(nameof(theaterRepository));
    }

    public async Task ExecuteAsync(List<Theater> theaters)
    {
        await theaterRepository.AddTheatersAsync(theaters);
    }
}
