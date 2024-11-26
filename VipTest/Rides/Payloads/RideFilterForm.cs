using VipTest.Users.customers;
using VipTest.Utlity.Basic;

namespace VipTest.Rides.Payloads;

public class RideFilterForm:BaseFilter
{
    public string? RidingCode { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? DriverId { get; set; }
    public Guid? VehicleId { get; set; }
    
    public string? PickupLocation { get; set; }
    public string? DropOffLocation { get; set; }
    public bool? IsDetour { get; set; }
    public DateTime? PickupTime { get; set; }
    public DateTime? CompletedAt { get; set; }
  
    // New property to indicate if we want to filter rides for today
    public bool? IsToday { get; set; }  
    
    
    
}