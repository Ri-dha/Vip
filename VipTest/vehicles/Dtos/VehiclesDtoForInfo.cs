using VipTest.Utlity.Basic;
using VipTest.vehicles.Utli;

namespace VipTest.vehicles.Dtos;

public class VehiclesDtoForInfo:BaseDto<Guid>
{
    
    public string? VehicleName { get; set; }
    public string? VehicleModel { get; set; }
    public string? VehicleNumber { get; set; }
    public string? VehicleColor { get; set; }
    public List<string>? VehicleImages { get; set; }
    public string? VehicleChassisNumber { get; set; }
    public int VehicleTripCount { get; set; }
    public int VehicleRating { get; set; }
    
    public VehicleType? VehicleType { get; set; }
    public string? VehicleTypeName => VehicleType.ToString();
    public CarBrand? VehicleBrand { get; set; }
    public string? VehicleBrandName => VehicleBrand.ToString();
    public CarType? CarType { get; set; }
    public string? CarTypeName => CarType.ToString();
    public ShifterType? ShifterType { get; set; }
    public string? ShifterTypeName => ShifterType.ToString();
    public VehicleStatus VehicleStatus { get; set; }
    public string? VehicleStatusName => VehicleStatus.ToString();
}