using VipTest.Utlity.Basic;
using VipTest.vehicles.Utli;

namespace VipTest.vehicles.PayLoads;


public class VehicleFilterForm : BaseFilter
{
    public string? VehicleName { get; set; }
    public string? VehicleModel { get; set; }
    public VehicleType? VehicleType { get; set; }
    
    public CarBrand? VehicleBrand { get; set; }
    public CarType? CarType { get; set; }
    public ShifterType? ShifterType { get; set; }

    public string? VehicleNumber { get; set; }
    public string? VehicleColor { get; set; }    
    public VehicleStatus? VehicleStatus { get; set; }
    
    public int? VehicleRating { get; set; }
    public int? VehicleCapacity { get; set; }
    public int? VehicleYear { get; set; }
    public string? VehicleLicensePlate { get; set; }
    public string? VehicleRegistration { get; set; }
    public Guid? WarehouseId { get; set; }
    public string? WarehouseName { get; set; }
    public bool? HasRentalPrice { get; set; }
    public decimal? StartRentalPrice { get; set; }
    public decimal? EndRentalPrice { get; set; }
    
    public int? StartYear { get; set; }
    public int? EndYear { get; set; }
    
    public CarAcceptanceStatus? CarAcceptanceStatus { get; set; }
    
    public Guid? UserId { get; set; }

}