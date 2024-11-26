using AutoMapper;
using VipTest.Data;
using VipTest.Utlity.Basic;
using VipTest.vehicles.Modles;

namespace VipTest.vehicles.Repositories;


public interface IVehiclesRepository:IBaseRepository<Vehicles,Guid>
{
    
}


public class VehiclesRepository:BaseRepository<Vehicles,Guid>,IVehiclesRepository
{
    
    private readonly VipProjectContext _db;

    public VehiclesRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    { _db = context; }
    
}