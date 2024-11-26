using VipTest.Rides.Utli;
using VipTest.Utlity.Basic;
using VipTest.vehicles.Utli;

namespace VipTest.RideBillings.Models;

public class RideBillingTypesConfig : BaseEntity<Guid>
{
    public RideType? RideType { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsEnabled => true;
    public decimal BaseFarePrice { get; set; }
    public decimal DetourFarePrice { get; set; }
}