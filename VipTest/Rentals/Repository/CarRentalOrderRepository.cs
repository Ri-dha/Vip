using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VipTest.Data;
using VipTest.Rentals.Models;
using VipTest.Utlity.Basic;

namespace VipTest.Rentals.Repository;

public interface ICarRentalOrderRepository:IBaseRepository<CarRentalOrder,Guid>
{
    Task<CarRentalOrder?> GetLatestCarRentalOrderAsync(string datePart);
    Task<bool> IsCarRentalOrderExist(string orderCode);

}

public class CarRentalOrderRepository:BaseRepository<CarRentalOrder,Guid>,ICarRentalOrderRepository
{
    private readonly VipProjectContext _db;

    public CarRentalOrderRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    { _db = context; }

    public async Task<CarRentalOrder?> GetLatestCarRentalOrderAsync(string datePart)
    {
        return await _db.Set<CarRentalOrder>()
            .Where(r => r.OrderCode != null && r.OrderCode.StartsWith(datePart))
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public Task<bool> IsCarRentalOrderExist(string orderCode)
    {
        return _db.Set<CarRentalOrder>().AnyAsync(r => r.OrderCode == orderCode);
    }
}