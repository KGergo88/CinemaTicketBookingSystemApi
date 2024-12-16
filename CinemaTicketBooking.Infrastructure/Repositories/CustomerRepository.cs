using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.Infrastructure.Repositories;

internal class CustomerRepository : ICustomerRepository
{
    private readonly IMapper mapper;
    private readonly CinemaTicketBookingDbContext context;

    public CustomerRepository(IMapper mapper, CinemaTicketBookingDbContext context)
    {
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Customer?> GetCustomerOrNullAsync(Guid customerId)
    {
        var infraCustomer = await context.Customers.SingleOrDefaultAsync(c => c.Id == customerId);
        return mapper.Map<Customer>(infraCustomer);
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        var infraCustomer = mapper.Map<CustomerEntity>(customer);
        context.Customers.Add(infraCustomer);
        await context.SaveChangesAsync();
    }
}
