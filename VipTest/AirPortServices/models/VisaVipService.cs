using VipTest.attachmentsConfig;

namespace VipTest.AirPortServices.models;

public class VisaVipService:AirportServicesModel
{
    public List<Attachments> Attachments { get; set; } = new List<Attachments>();
}