using VipTest.attachmentsConfig;
using VipTest.Rides.Utli;

namespace VipTest.AirPortServices.Payloads;

public class AirportServicesCreateForm
{
    public int NumberOfCustomers { get; set; }
    public Guid? CustomerId { get; set; }
    public string? Note { get; set; }
    public RidePaymentType? PaymentType { get; set; }
    public string? DiscountCode { get; set; }
}

public class LoungeServiceCreateForm : AirportServicesCreateForm
{
}

public class LuggageServiceCreateForm : AirportServicesCreateForm
{
}

public class VisaVipServiceCreateForm : AirportServicesCreateForm
{
    public List<AttachmentForm> Attachments { get; set; } 
}