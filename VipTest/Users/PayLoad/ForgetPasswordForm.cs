using System.ComponentModel.DataAnnotations;

namespace VipTest.Users.PayLoad;

public class ForgetPasswordForm
{
    [Required(ErrorMessage = "Phone Number is required.")]
    public string PhoneNumber { get; set; }
}

public class ChangePasswordForm
{
    [Required(ErrorMessage = "New Password is required.")]
    public string NewPassword { get; set; }
    
    [Required(ErrorMessage = "Phone Number is required.")]
    public string PhoneNumber { get; set; }
}

public class VerifyOtpForm
{
    [Required(ErrorMessage = "Phone Number is required.")]
    public string PhoneNumber { get; set; }
    
    [Required(ErrorMessage = "OTP is required.")]
    public string Otp { get; set; }
}