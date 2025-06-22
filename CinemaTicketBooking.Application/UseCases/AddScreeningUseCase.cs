using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

internal class AddScreeningUseCase : IAddScreeningUseCase
{
    private readonly IScreeningRepository screeningRepository;

    public AddScreeningUseCase(IScreeningRepository screeningRepository)
    {
        this.screeningRepository = screeningRepository ?? throw new ArgumentNullException(nameof(screeningRepository));
    }

    public async Task ExecuteAsync(Screening screening)
    {
        await screeningRepository.AddScreeningsAsync([screening]);
    }
}
