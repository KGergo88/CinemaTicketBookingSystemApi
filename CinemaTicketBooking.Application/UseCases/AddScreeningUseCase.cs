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

        // We must ensure that the minimum cleanup duration of the auditorium is respected between screenings
        // This means that in order to be able to create this screening,
        // the last one shall end at least a pause length before this one starts
        // and the next one shall start at least a pause length after this one ends
        var timeFrameStart = screening.Showtime - auditorium.MinimumCleanupDuration;
        var timeFrameDuration = movie.Duration + (auditorium.MinimumCleanupDuration * 2);
        var overlappingScreeningIds = await screeningRepository.FindScreeningIdsInTimeFrameAsync(auditorium.Id, timeFrameStart, timeFrameDuration);
        if (overlappingScreeningIds.Count != 0)
        {
            var commaSeparatedIds = string.Join(',', overlappingScreeningIds);
            throw new ConflictException("There are overlapping screenings in the same auditorium!" +
                                        $" AuditoriumId: {auditorium.Id}, IDs of overlapping screenings: [{commaSeparatedIds}]");
        }

        await screeningRepository.AddScreeningsAsync([screening]);
    }
}
