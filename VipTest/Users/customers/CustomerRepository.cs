using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VipTest.Data;
using VipTest.Utlity;
using VipTest.Utlity.Basic;

namespace VipTest.Users.customers;

public interface ICustomerRepository : IBaseRepository<Customer, Guid>
{
    // Add any customer-specific methods here if needed

    Task<List<Customer>> GetAllCustomers();
}

public class CustomerRepository : BaseRepository<Customer, Guid>, ICustomerRepository
{
    private readonly VipProjectContext _context;

    public CustomerRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }

    // Implement any customer-specific methods here if needed
    public async Task<List<Customer>> GetAllCustomers()

    {
        return await _context.Customers.ToListAsync();
    }
}