namespace CinemaTicketBooking.Application.UseCases;

public interface IDeleteMovieUseCase
{
    public Task ExecuteAsync(List<Guid> movieIdsToDelete);
}
