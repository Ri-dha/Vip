using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VipTest.Data;
using VipTest.Utlity;
using VipTest.Utlity.Basic;

namespace VipTest.Users.Admins;

public interface IAdminRepository:IBaseRepository<Admin,Guid>
{
    Task<List<Admin>> getAllAdmins();
}

public class AdminRepository : BaseRepository<Admin, Guid>, IAdminRepository
{
    private readonly VipProjectContext _context;


    public AdminRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }

    public async Task<List<Admin>> getAllAdmins()
    {
        return await _context.Admins.ToListAsync();
    }
}