using VipTest.Utlity.Basic;

namespace VipTest.Users.OTP;

public class PendingCustomer:BaseEntity<Guid>
{
    public string? Username { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
    public Boolean? IsOtPVerified = false;
    
    public PendingCustomer(string username, string phoneNumber, string password)
    {
        Username = username;
        PhoneNumber = phoneNumber;
        Password = password;
    }
}