using VipTest.Rentals.utli;
using VipTest.Utlity.Basic;

namespace VipTest.Rentals.Dto;

public class CarRentalOrderDtoForInfo:BaseDto<Guid>
{
    public string? OrderCode { get; set; }
    public Guid VehicleId { get; set; }
    public string? VehicleName { get; set; }
    public string? VehiclePlateNumber { get; set; }
    public Guid DriverId { get; set; }
    public string? DriverName { get; set; }
    public Guid CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public RentalOrderStatus? Status { get; set; }
    public decimal FinalPrice { get; set; }
    public DateTime PickupTime { get; set; }
    public DateTime DropOffTime { get; set; }
    public DateTime? CarBackTime { get; set; }
}