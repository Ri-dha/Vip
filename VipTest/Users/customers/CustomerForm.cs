using VipTest.Users.PayLoad;

namespace VipTest.Users.customers;

public class CustomerForm : UserForm
{
    public string? CustomerIdFile { get; set; }  // File upload for customer ID
    public string? CustomerLicenseFile { get; set; }  // File upload for license
}