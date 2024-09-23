using CinemaTicketBooking.Application.Interfaces.Repositories;

namespace CinemaTicketBooking.Application.UseCases;

internal class DeleteMovieUseCase : IDeleteMovieUseCase
{
    private readonly IMovieRepository movieRepository;

    public DeleteMovieUseCase(IMovieRepository movieRepository)
    {
        this.movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
    }

    public async Task ExecuteAsync(List<Guid> movieIdsToDelete)
    {
        await movieRepository.DeleteMoviesAsync(movieIdsToDelete);
    }
}
