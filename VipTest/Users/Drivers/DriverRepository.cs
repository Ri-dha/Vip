using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VipTest.Data;
using VipTest.Users.Drivers.Models;
using VipTest.Utlity;
using VipTest.Utlity.Basic;

namespace VipTest.Users.Drivers;


public interface IDriverRepository : IBaseRepository<Driver, Guid>
{
    // You can add specific methods related to Driver here if needed
    Task<List<Driver>> GetAllDrivers();
}

public class DriverRepository : BaseRepository<Driver, Guid>, IDriverRepository
{
    private readonly VipProjectContext _context;

    public DriverRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }

    // Implement any additional driver-specific methods if needed
    public async Task<List<Driver>> GetAllDrivers()
    {
        return await _context.Drivers.ToListAsync();
    }
}