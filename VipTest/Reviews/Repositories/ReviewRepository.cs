using AutoMapper;
using VipTest.Data;
using VipTest.reviews.models;
using VipTest.Utlity.Basic;

namespace VipTest.reviews.Repositories;

public interface IReviewRepository:IBaseRepository<Review,Guid>
{
    
    
}

public class ReviewRepository:BaseRepository<Review,Guid>,IReviewRepository
{
    
    private readonly VipProjectContext _db;

    public ReviewRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    { _db = context; }
    
}