using VipTest.Users.PayLoad;

namespace VipTest.Users.Admins;

public class AdminForm: UserForm
{
    public string? AdminIdFile { get; set; }  // File upload for the admin's ID
    public AdministrativeRoles? AdministrativeRole { get; set; } // Admin's role
}