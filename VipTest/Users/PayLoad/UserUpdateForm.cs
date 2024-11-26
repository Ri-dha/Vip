using VipTest.Users.Models;

namespace VipTest.Users.PayLoad;

public class UserUpdateForm
{
    public string? Username { get; set; }


    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public Roles? Role { get; set; }
    public string? ProfileImage { get; set; }
}