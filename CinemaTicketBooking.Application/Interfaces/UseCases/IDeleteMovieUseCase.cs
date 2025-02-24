namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public interface IDeleteMovieUseCase
{
    public Task ExecuteAsync(IEnumerable<Guid> movieIdsToDelete);
}
