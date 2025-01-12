using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

internal class AddScreeningsUseCase : IAddScreeningsUseCase
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
