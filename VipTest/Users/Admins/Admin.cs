using VipTest.Users.Models;
using VipTest.Utlity;

namespace VipTest.Users.Admins;

public class Admin:User
{
    public string? AdminIdFile { get; set; } // Navigation property for the ID file
    
    public AdministrativeRoles? AdministrativeRole { get; set; }
}