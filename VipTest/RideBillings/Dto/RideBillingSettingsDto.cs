using VipTest.Rides.Utli;
using VipTest.Utlity.Basic;

namespace VipTest.RideBillings.Dto;

public class RideBillingSettingsDto
{
    public RideType RideType { get; set; }
    public decimal BaseFarePrice { get; set; }
    public decimal DetourFarePrice { get; set; }

    
}