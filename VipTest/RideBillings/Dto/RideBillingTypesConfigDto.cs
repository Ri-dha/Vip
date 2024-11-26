using VipTest.Rides.Utli;
using VipTest.Utlity.Basic;

namespace VipTest.RideBillings.Dto;

public class RideBillingTypesConfigDto:BaseDto<Guid>
{
    public RideType RideType { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsEnabled { get; set; }
    public decimal BaseFarePrice { get; set; }
    public decimal DetourFarePrice { get; set; }

}