using VipTest.Rides.Utli;

namespace VipTest.RideBillings.Payloads;

public class RideBillingTypesConfigCreateForm
{
    public RideType? RideType { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal BaseFarePrice { get; set; }
    public decimal DetourFarePrice { get; set; }
}