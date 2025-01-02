namespace CinemaTicketBooking.Application.Interfaces.UseCases;

public interface IDeleteMovieUseCase
{
    public Task ExecuteAsync(List<Guid> movieIdsToDelete);
}
