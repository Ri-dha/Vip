using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VipTest.Data;
using VipTest.Notifications.Models;
using VipTest.Utlity.Basic;

namespace VipTest.Notifications.Repositories;

public interface INotificationTemplateRepository : IBaseRepository<NotificationTemplate, Guid>
{
    Task<NotificationTemplate?> getLatest(string datePart);
}

public class NotificationTemplateRepository : BaseRepository<NotificationTemplate, Guid>,
    INotificationTemplateRepository
{
    private readonly VipProjectContext _db;

    public NotificationTemplateRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _db = context;
    }


    public async Task<NotificationTemplate?> getLatest(string datePart)
    {
        return await _db.Set<NotificationTemplate>()
            .Where(r => r.NotificationCode != null && r.NotificationCode.StartsWith(datePart))
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefaultAsync();
    }
}

public interface IUserNotificationRepository : IBaseRepository<UserNotification, Guid>
{
}

public class UserNotificationRepository : BaseRepository<UserNotification, Guid>, IUserNotificationRepository
{
    private readonly VipProjectContext _db;

    public UserNotificationRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _db = context;
    }
}

public interface IUserGroupsRepository : IBaseRepository<UserGroups, Guid>
{
}

public class UserGroupsRepository : BaseRepository<UserGroups, Guid>, IUserGroupsRepository
{
    private readonly VipProjectContext _db;

    public UserGroupsRepository(VipProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _db = context;
    }
}