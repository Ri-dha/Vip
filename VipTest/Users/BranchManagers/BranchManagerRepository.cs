using AutoMapper;
using VipTest.Data;
using VipTest.Utlity.Basic;

namespace VipTest.Users.BranchManagers;


public interface IBranchManagerRepository: IBaseRepository<BranchManager,Guid>
{
    
}


public class BranchManagerRepository: BaseRepository<BranchManager,Guid>, IBranchManagerRepository
{
    private readonly VipProjectContext _context;

    public BranchManagerRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }
    
}