using AutoMapper;
using VipTest.Data;
using VipTest.reviews.models;
using VipTest.Utlity.Basic;

namespace VipTest.reviews.Repositories;

public interface IRideReviewRepository : IBaseRepository<RideReview, Guid>
{
}

public class RideReviewRepository : BaseRepository<RideReview, Guid>, IRideReviewRepository
{
    private readonly VipProjectContext _db;

    public RideReviewRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _db = context;
    }
}