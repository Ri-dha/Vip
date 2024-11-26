using VipTest.attachmentsConfig;
using VipTest.Users.PayLoad;

namespace VipTest.Users.Drivers.PayLoads;

public class DriverForm : UserForm
{
    public string? userNameAr { get; set; }
    public string? DriverLicense { get; set; } // File upload for the driver's license
    public string? DriverIdFile { get; set; } // File upload for the driver's ID
    // Add attachments to the DriverUpdateForm
    public List<AttachmentForm>? Attachments { get; set; }
}