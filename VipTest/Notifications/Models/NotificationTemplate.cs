using VipTest.Users.Admins;
using VipTest.Utlity.Basic;

namespace VipTest.Notifications.Models;

public class NotificationTemplate:BaseEntity<Guid>
{
    public string? NotificationCode { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public NotificationType Type { get; set; }
    public NotificationStatus Status { get; set; }
    public Guid? SenderId { get; set; }
    public Admin? Sender { get; set; }
    public List<Guid> SendTo { get; set; }= new List<Guid>();
    public List<UserGroups>?  UserGroups { get; set; }
    public DateTime? ScheduledTime { get; set; }
    
}

public enum NotificationType
{
    Info=1,
    Warning=2,
    Error=3,
    Discount=4,
    Promotion=5,
    SystemUpdate=6,
    Other=7
}

public enum NotificationStatus
{
    Sent=1,
    Scheduled=2
}