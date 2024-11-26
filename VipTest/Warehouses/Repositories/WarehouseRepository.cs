using AutoMapper;
using VipTest.Data;
using VipTest.Utlity.Basic;
using VipTest.Warehouses.Models;

namespace VipTest.Warehouses;

public interface IWarehouseRepository:IBaseRepository<Warehouse,Guid>
{
    
}

public class WarehouseRepository:BaseRepository<Warehouse,Guid>,IWarehouseRepository
{
    
    private readonly VipProjectContext _db;

    public WarehouseRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    { _db = context; }
}