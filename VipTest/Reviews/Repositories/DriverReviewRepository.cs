using AutoMapper;
using VipTest.Data;
using VipTest.reviews.models;
using VipTest.Utlity.Basic;

namespace VipTest.reviews.Repositories;

public interface IDriverReviewRepository:IBaseRepository<DriverReview,Guid>
{
    
}


public class DriverReviewRepository:BaseRepository<DriverReview,Guid>,IDriverReviewRepository
{
    
    private readonly VipProjectContext _db;

    public DriverReviewRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    { _db = context; }
    
}
    
