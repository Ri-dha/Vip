using AutoMapper;
using VipTest.Data;
using VipTest.Utlity.Basic;
using VipTest.Wallets.Model;

namespace VipTest.Wallets.Repositories;


public interface IWalletRepository: IBaseRepository<Wallet,Guid>
{
    
}


public class WalletRepository:BaseRepository<Wallet,Guid>,IWalletRepository
{
    
    private readonly VipProjectContext _db;
    
    public WalletRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    { _db = context; }
    
}