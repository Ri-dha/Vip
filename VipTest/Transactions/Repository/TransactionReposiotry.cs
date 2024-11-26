using AutoMapper;
using VipTest.Data;
using VipTest.Transactions.models;
using VipTest.Utlity.Basic;

namespace VipTest.Transactions.Repository;


public interface ITransactionReposiotry: IBaseRepository<Transaction,Guid>
{
    
}


public class TransactionReposiotry:BaseRepository<Transaction,Guid>,ITransactionReposiotry
{
    
    private readonly VipProjectContext _db;
    
    public TransactionReposiotry(VipProjectContext context, IMapper mapper) : base(context, mapper)
    { _db = context; }
    
}