using AutoMapper;
using VipTest.Data;
using VipTest.Files.Models;
using VipTest.Utlity;
using VipTest.Utlity.Basic;

namespace VipTest.Files;


public interface IFileRepository : IBaseRepository<ProjectFiles, Guid>
{
    // Additional methods specific to the File entity can be added here
}
public class FileRepository: BaseRepository<ProjectFiles, Guid>, IFileRepository
{
    private readonly VipProjectContext _context;

    public FileRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }
}