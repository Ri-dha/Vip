using AutoMapper;
using VipTest.Data;
using VipTest.FavPlaces.models;
using VipTest.Utlity.Basic;

namespace VipTest.FavPlaces.Repositories;


public interface IFavouritePlaceRepository:IBaseRepository<FavouritePlace,Guid>
{
    
}
public class FavouritePlaceRepository:BaseRepository<FavouritePlace,Guid>,IFavouritePlaceRepository
{
    
    private readonly VipProjectContext _db;

    public FavouritePlaceRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    { _db = context; }
    
}