using VipTest.Notifications.Models;
using VipTest.Users.Admins;
using VipTest.Utlity.Basic;

namespace VipTest.Notifications.Dtos;

public class NotificationTemplateDto:BaseDto<Guid>
{
    public string? NotificationCode { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public NotificationType? Type { get; set; }
    public NotificationStatus? Status { get; set; }
    public Guid? SenderId { get; set; }
    public AdminDto? Sender { get; set; }
    public List<Guid>? SendTo { get; set; }
    public List<UserGroupsDtoForInfo>? UserGroups { get; set; }
    public DateTime? ScheduledTime { get; set; }
}