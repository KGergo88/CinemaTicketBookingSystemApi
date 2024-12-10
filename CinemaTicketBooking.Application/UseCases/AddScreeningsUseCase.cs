using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

internal class AddScreeningsUseCase
{
    private readonly IScreeningRepository screeningRepository;

    public AddScreeningsUseCase(IScreeningRepository screeningRepository)
    {
        this.screeningRepository = screeningRepository ?? throw new ArgumentNullException(nameof(screeningRepository));
    }

    public async Task ExecuteAsync(List<Screening> screenings)
    {
        await screeningRepository.AddScreeningsAsync(screenings);
    }
}
