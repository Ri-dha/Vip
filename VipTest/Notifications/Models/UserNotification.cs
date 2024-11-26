using VipTest.Users.Admins;
using VipTest.Users.Models;
using VipTest.Utlity.Basic;

namespace VipTest.Notifications.Models;

public class UserNotification:BaseEntity<Guid>
{
    public string? Title { get; set; }
    public string? Description { get; set; }    
    public Guid? ReceiverId { get; set; }
    public bool IsRead { get; set; } = false;
}


