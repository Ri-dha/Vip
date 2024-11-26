using AutoMapper;
using VipTest.Data;
using VipTest.reviews.models;
using VipTest.Utlity.Basic;

namespace VipTest.reviews.Repositories;


public interface IVehicleReviewRepository:IBaseRepository<VehicleReview,Guid>
{
    
}

public class VehicleReviewRepository:BaseRepository<VehicleReview,Guid>,IVehicleReviewRepository
{
        
        private readonly VipProjectContext _db;
    
        public VehicleReviewRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
        { _db = context; }
    
}