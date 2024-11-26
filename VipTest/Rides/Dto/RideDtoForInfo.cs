using VipTest.Rides.Utli;
using VipTest.Utlity.Basic;

namespace VipTest.Rides.Dto;

public class RideDtoForInfo:BaseDto<Guid>
{
    public string? RideCode { get; set; }
    public Guid DriverId { get; set; }
    public string? DriverName { get; set; }
    
    public Guid CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public RideStatus Status { get; set; }
    public decimal FinalPrice { get; set; }
    
    public DateTime? PickupTime { get; set; }

}