using VipTest.Users.Admins;
using VipTest.Users.customers;
using VipTest.Users.Drivers;
using VipTest.Users.Models;
using VipTest.Utlity;
using VipTest.Utlity.Basic;

namespace VipTest.Users.PayLoad;

public class UserFilter : BaseFilter
{
    public string? Email { get; set; }

    public string? UserName { get; set; }
    public Roles? Role { get; set; }
    
    public string? PhoneNumber { get; set; }
    
}


public class CustomerFilter:UserFilter
{
    
    public CustomerStatus? CustomerStatus { get; set; }
    
    
}

public class AdminFilter:UserFilter
{
    
    public AdministrativeRoles? AdministrativeRole { get; set; }    
    
}


public class DriverFilter:UserFilter
{
    
    public DriverStatus? DriverStatus { get; set; }
    
    
}