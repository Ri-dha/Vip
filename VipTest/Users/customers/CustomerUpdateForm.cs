using VipTest.Users.PayLoad;

namespace VipTest.Users.customers;

public class CustomerUpdateForm : UserUpdateForm
{
    public string? CustomerIdFile { get; set; }          // Optional: Update ID file path
    public string? CustomerLicenseFile { get; set; }     // Optional: Update license file path
    public bool? IsVerified { get; set; }        // Optional: Update verification status
    
    public CustomerStatus? CustomerStatus { get; set; }
    
    public bool? IsEnglish { get; set; }        
    public bool IsUsd { get; set; } 
}