using VipTest.Rides.Utli;

namespace VipTest.Discounts.Payloads;

public class DiscountCheckForm
{
   public string DiscountCode { get; set; }
   public Guid UserId { get; set; }
   public RideType RideType { get; set; }
}