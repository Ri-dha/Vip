using AutoMapper;
using VipTest.Data;
using VipTest.RideBillings.Models;
using VipTest.Utlity.Basic;

namespace VipTest.RideBillings.Repositories;


public interface IRideBillingRepository:IBaseRepository<RideBillingTypesConfig,Guid>
{
    
}


public class RideBillingRepository:BaseRepository<RideBillingTypesConfig,Guid>,IRideBillingRepository
{
    
    private readonly VipProjectContext _db;
    
    public RideBillingRepository(VipProjectContext context,IMapper mapper) : base(context,mapper)
    { _db = context; }
}