using AutoMapper;
using VipTest.AirPortServices.models;
using VipTest.AirPortServices.Utli;
using VipTest.Data;
using VipTest.Utlity.Basic;

namespace VipTest.AirPortServices.repoistores;

public interface IAirportServicesRepoistory : IBaseRepository<AirportServicesModel, Guid>
{
}

public class AirportServicesRepoistory : BaseRepository<AirportServicesModel, Guid>, IAirportServicesRepoistory
{
    private readonly VipProjectContext _db;

    public AirportServicesRepoistory(VipProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _db = context;
    }
}

public interface ILuggageRepository : IBaseRepository<LuggageService, Guid>
{
}

public class LuggageRepository : BaseRepository<LuggageService, Guid>, ILuggageRepository
{
    private readonly VipProjectContext _db;

    public LuggageRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _db = context;
    }
}

public interface ILoungeRepository : IBaseRepository<LoungeService, Guid>
{
}

public class LoungeRepository : BaseRepository<LoungeService, Guid>, ILoungeRepository
{
    private readonly VipProjectContext _db;

    public LoungeRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _db = context;
    }
}

public interface IVisaVipRepository : IBaseRepository<VisaVipService, Guid>
{
}

public class VisaVipRepository : BaseRepository<VisaVipService, Guid>, IVisaVipRepository
{
    private readonly VipProjectContext _db;

    public VisaVipRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _db = context;
    }
}