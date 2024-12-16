using CinemaTicketBooking.Domain.Entities;

namespace CinemaTicketBooking.Application.Interfaces.Repositories;

public interface ICustomerRepository
{
    public Task<Customer?> GetCustomerOrNullAsync(Guid id);

    public Task AddCustomerAsync(Customer customer);
}
