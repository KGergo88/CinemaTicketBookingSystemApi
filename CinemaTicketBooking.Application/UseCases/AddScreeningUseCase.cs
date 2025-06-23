using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;
using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.UseCases;

internal class AddScreeningUseCase : IAddScreeningUseCase
{
    private readonly IScreeningRepository screeningRepository;
    private readonly ITheaterRepository theaterRepository;
    private readonly IMovieRepository movieRepository;

    public AddScreeningUseCase(IScreeningRepository screeningRepository,
                               ITheaterRepository theaterRepository,
                               IMovieRepository movieRepository)
    {
        this.screeningRepository = screeningRepository ?? throw new ArgumentNullException(nameof(screeningRepository));
        this.theaterRepository = theaterRepository ?? throw new ArgumentNullException(nameof(theaterRepository));
        this.movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
    }

    public async Task ExecuteAsync(Screening screening)
    {
        var movie = await movieRepository.GetMovieOrNullAsync(screening.MovieId);
        if (movie is null)
            throw new NotFoundException($"No movie with the ID {screening.MovieId} was found!");

        var auditorium = await theaterRepository.GetAuditoriumOrNullAsync(screening.AuditoriumId);
        if (auditorium is null)
            throw new NotFoundException($"No auditorium with the ID {screening.AuditoriumId} was found!");

        await screeningRepository.AddScreeningsAsync([screening]);
    }
}
