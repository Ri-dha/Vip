using VipTest.Utlity.Basic;

namespace VipTest.Notifications.Dtos;

public class UserGroupsDto:BaseDto<Guid>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public List<Guid>? userIds { get; set; }
}

public class UserGroupsDtoForInfo:BaseDto<Guid>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
}