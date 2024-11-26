using VipTest.AirPortServices.Utli;
using VipTest.attachmentsConfig;

namespace VipTest.AirPortServices.Payloads;

public class AirportServicesUpdateForm
{
    public string? DiscountCode { get; set; }
}

public class LoungeUpdateForm : AirportServicesUpdateForm
{
}

public class VisaVipUpdateForm : AirportServicesUpdateForm
{
    public List<AttachmentForm>? Attachments { get; set; } 
}

public class LuggageUpdateForm : AirportServicesUpdateForm
{
}