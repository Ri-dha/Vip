using AutoMapper;
using VipTest.AppSettings.models;
using VipTest.Data;
using VipTest.Utlity.Basic;

namespace VipTest.AppSettings.repo;

public interface ISettingsRepository: IBaseRepository<Settings,Guid>
{
    
}
public class SettingsRepository:BaseRepository<Settings,Guid>,ISettingsRepository
{
    
    
    private readonly VipProjectContext _db;
    
    public SettingsRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    { _db = context; }
    
}