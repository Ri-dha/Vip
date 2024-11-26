using System.ComponentModel.DataAnnotations;

namespace VipTest.Users.OTP;

public class PendingCustomerForm
{
    [Required(ErrorMessage = "Username is required.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Phone Number is required.")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }
}