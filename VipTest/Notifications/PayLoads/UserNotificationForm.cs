using VipTest.Utlity.Basic;

namespace VipTest.Notifications.PayLoads;

public class UserNotificationForm
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid? ReceiverId { get; set; }
}

public class UserNotificationFilter : BaseFilter
{
    public Guid? ReceiverId { get; set; }
    public bool? IsRead { get; set; }
    public DateTime? DateTimeFrom { get; set; }
    public DateTime? DateTimeTo { get; set; }
    
}

public class UserNotificationUpdateForm
{
    public bool IsRead { get; set; }
}