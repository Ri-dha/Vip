using VipTest.Users.Models;
using VipTest.Utlity.Basic;

namespace VipTest.Notifications.Models;

public class UserGroups:BaseEntity<Guid>
{
    public string? code { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public List<Guid> userIds { get; set; }=new List<Guid>();
}