using AutoMapper;
using VipTest.Data;
using VipTest.Discounts.Models;
using VipTest.Utlity.Basic;

namespace VipTest.Discounts.repositories;

public interface IDiscountRepository:IBaseRepository<Discount,Guid>
{
    
}

public class DiscountRepository:BaseRepository<Discount,Guid>,IDiscountRepository
{
        
        private readonly VipProjectContext _db;
    
        public DiscountRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
        { _db = context; }
    
}