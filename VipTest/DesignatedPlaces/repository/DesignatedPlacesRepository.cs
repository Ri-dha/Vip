using AutoMapper;
using VipTest.Data;
using VipTest.Utlity.Basic;

namespace VipTest.DesignatedPlaces.repository;

public interface IDesignatedPlacesRepository:IBaseRepository<model.DesignatedPlaces,Guid>
{
    
}

public class DesignatedPlacesRepository:BaseRepository<model.DesignatedPlaces,Guid>,IDesignatedPlacesRepository
{
    private readonly VipProjectContext _db;
    
    public DesignatedPlacesRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    { _db = context; }
    
    
}