using VipTest.reviews.utli;
using VipTest.Utlity.Basic;

namespace VipTest.reviews.Payloads;

public class ReviewFilterForm:BaseFilter
{
    public Guid? CustomerId { get; set; }
    public Guid? DriverId { get; set; }
    public Guid? VehicleId { get; set; }
    public Guid? RideId { get; set; }
    public ReviewFor? ReviewFor { get; set; }
    
}