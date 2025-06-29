using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

internal class SetPricingUseCase : ISetPricingUseCase
{
    private readonly IScreeningRepository screeningRepository;
    private readonly ITheaterRepository theaterRepository;

    public SetPricingUseCase(IScreeningRepository screeningRepository,
                             ITheaterRepository theaterRepository)
    {
        this.screeningRepository = screeningRepository ?? throw new ArgumentNullException(nameof(screeningRepository));
        this.theaterRepository = theaterRepository ?? throw new ArgumentNullException(nameof(theaterRepository));
    }

    public async Task ExecuteAsync(Pricing pricing)
    {
        var screening = await screeningRepository.GetScreeningOrNullAsync(pricing.ScreeningId);
        if (screening is null)
            throw new NotFoundException($"No screening with the ID {pricing.ScreeningId} was found!");

        var tier = await theaterRepository.GetTierOrNullAsync(pricing.TierId);
        if (tier is null)
            throw new NotFoundException($"No tier with the ID {pricing.TierId} was found!");

        await screeningRepository.SetPricingAsync(pricing);
    }
}