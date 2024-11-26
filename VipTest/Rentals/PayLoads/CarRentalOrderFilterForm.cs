using VipTest.Rentals.utli;
using VipTest.Utlity.Basic;

namespace VipTest.Rentals.PayLoads;

public class CarRentalOrderFilterForm:BaseFilter
{
    public string? OrderCode { get; set; }
    public Guid? VehicleId { get; set; }
    public Guid? DriverId { get; set; }
    public Guid? CustomerId { get; set; }
    public RentalOrderStatus? Status { get; set; }
    public DateTime? PickupTime { get; set; }
    public DateTime? DropOffTime { get; set; }
    public DateTime? CarBackTime { get; set; }
    public PickUpType? PickUpType { get; set; } 
    public bool? IsToday { get; set; }  
    public Guid? WarehouseId { get; set; }
}