using VipTest.Notifications.Models;
using VipTest.Utlity.Basic;

namespace VipTest.Notifications.PayLoads;

public class NotificationTemplateForm
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public NotificationType Type { get; set; }
    public List<Guid>? UserGroupId { get; set; }
    public List<Guid>? SendTo { get; set; }
    public DateTime? ScheduledTime { get; set; }
}

public class NotificationTemplateUpdateForm
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public NotificationType? Type { get; set; }
    public Guid? SenderId { get; set; }
    public List<Guid>? UserGroupId { get; set; }
    public List<Guid>? SendTo { get; set; }
    public DateTime? ScheduledTime { get; set; }
}

public class NotificationTemplateFilterForm:BaseFilter
{
    
    public string? NotificationCode { get; set; }
    public string? Title { get; set; }
    public NotificationType? Type { get; set; }
    public NotificationStatus? Status { get; set; }
    public DateTime? ScheduledTimeFrom { get; set; }
    public DateTime? ScheduledTimeTo { get; set; }
}