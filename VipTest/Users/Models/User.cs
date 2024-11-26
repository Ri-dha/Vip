using VipTest.Files.Models;
using VipTest.Utlity;
using VipTest.Utlity.Basic;
using VipTest.Wallets.Model;

namespace VipTest.Users.Models;

public class User:BaseEntity<Guid>
{
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string Password { get; set; }
    public Roles? Role { get; set; }
    public DateTime? LastLogin { get; set; }
    public string? ProfileImage { get; set; }

    public List<Wallet> Wallets { get; set; } = new List<Wallet>();

    public User()
    {
    }
    
    public User(string username, string phoneNumber, string email, string password)
    {
        Username = username;
        PhoneNumber = phoneNumber;
        Email = email;
        Password = password;
    }
}