using VipTest.Utlity.Basic;

namespace VipTest.Notifications.PayLoads;

public class UserGroupsForm
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public List<Guid>? userIds { get; set; }
}

public class UserGroupsFilter:BaseFilter
{
    public string? Title { get; set; }
}

public class UserGroupsUpdateForm
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public List<Guid>? userIds { get; set; }
}
