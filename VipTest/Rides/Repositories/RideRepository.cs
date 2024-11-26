using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VipTest.Data;
using VipTest.Rides.Models;
using VipTest.Utlity.Basic;

namespace VipTest.Rides.Repositories;

public interface IRideRepository:IBaseRepository<Ride,Guid>
{
    Task<Ride?> GetLatestRideAsync(string datePart);
    Task<bool> IsRideExist(string rideCode);
}


public class RideRepository:BaseRepository<Ride,Guid>,IRideRepository
{
    
    private readonly VipProjectContext _db;

    public RideRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    { _db = context; }

    public async Task<Ride?> GetLatestRideAsync(string datePart)
    {
        return await _db.Set<Ride>()
            .Where(r => r.RidingCode != null && r.RidingCode.StartsWith(datePart))
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public Task<bool> IsRideExist(string rideCode)
    {
        return _db.Set<Ride>().AnyAsync(r => r.RidingCode == rideCode);
    }
}