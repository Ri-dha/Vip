using VipTest.Users.PayLoad;

namespace VipTest.Users.Admins;

public class AdminUpdateForm:UserUpdateForm
{
    public string? AdminIdFile { get; set; }  // Optional: Update admin's ID file path
    public AdministrativeRoles? AdministrativeRole { get; set; } // Optional: Update admin's role
}