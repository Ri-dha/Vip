using System.ComponentModel.DataAnnotations;
using VipTest.Users.Models;

namespace VipTest.Users.PayLoad;

public class UserForm
{
    [Required(ErrorMessage = "Username is required.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Phone Number is required.")]
    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string? Password { get; set; }
    
    public string? ProfileImage { get; set; }

    
    // public Roles Role { get; set; }
}