using VipTest.Utlity.Basic;

namespace VipTest.Notifications.Dtos;

public class UserNotificationDto:BaseDto<Guid>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsRead { get; set; }    
}