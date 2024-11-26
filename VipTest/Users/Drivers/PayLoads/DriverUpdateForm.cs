using VipTest.attachmentsConfig;
using VipTest.Users.PayLoad;

namespace VipTest.Users.Drivers.PayLoads;

public class DriverUpdateForm : UserUpdateForm
{
    public string? DriverLicenseFile { get; set; } // Optional: Update driver's license file path
    public string? DriverIdFile { get; set; } // Optional: Update ID file path
    public DriverStatus? DriverStatus { get; set; } // Optional: Update driver status

    // Add attachments to the DriverUpdateForm
    public List<AttachmentForm>? Attachments { get; set; }
}