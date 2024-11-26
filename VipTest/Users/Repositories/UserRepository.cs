using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VipTest.Data;
using VipTest.Users.Dtos;
using VipTest.Users.Models;
using VipTest.Users.PayLoad;
using VipTest.Utlity;
using VipTest.Utlity.Basic;

namespace VipTest.Users.Repositories;


public interface IUserRepository : IBaseRepository<User, Guid> {
    
    Task<List<Guid>> GetUsersIdsByRole(Roles role);
}

public class UserRepository : BaseRepository<User, Guid>, IUserRepository {
    private readonly VipProjectContext _db;

    public UserRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    { _db = context; }


    public Task<List<Guid>> GetUsersIdsByRole(Roles role)
    {
        return _db.Users.Where(x => x.Role == role).Select(x => x.Id).ToListAsync();
    }
}