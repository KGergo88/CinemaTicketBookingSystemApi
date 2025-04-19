using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

internal class SetPricingsUseCase : ISetPricingUseCase
{
    private readonly IScreeningRepository screeningRepository;

    public SetPricingsUseCase(IScreeningRepository screeningRepository)
    {
        this.screeningRepository = screeningRepository ?? throw new ArgumentNullException(nameof(screeningRepository));
    }

    public async Task ExecuteAsync(Pricing pricing)
    {
        await screeningRepository.SetPricingAsync(pricing);
    }
}