using VipTest.attachmentsConfig;
using VipTest.vehicles.Utli;

namespace VipTest.vehicles.PayLoads;

public class VehicleCreateForm
{
    public string? GeneralDescription { get; set; }
    public string? VehicleName { get; set; }
    public string? VehicleNameAr { get; set; }
    public string? VehicleModel { get; set; }
    public string? VehicleModelAr { get; set; }
    public string? VehicleColor { get; set; }
    public string? VehicleColorAr { get; set; }
    public VehicleType? VehicleType { get; set; }
    public string? VehicleNumber { get; set; }
    public List<string>? VehicleImages { get; set; }
    public string? VehicleChassisNumber { get; set; }
    public int? VehicleCapacity { get; set; }
    public int? VehicleYear { get; set; }
    public string? VehicleLicensePlate { get; set; }
    public string? VehicleRegistration { get; set; }
    public decimal? RentalPrice { get; set; }
    public CarBrand? VehicleBrand { get; set; }
    public CarType? CarType { get; set; }
    public ShifterType? ShifterType { get; set; }
    public List<AttachmentForm>? Attachments { get; set; }
}

public class RentCarsCreateForm
{
    public string? GeneralDescription { get; set; }
    public string? VehicleName { get; set; }
    public string? VehicleNameAr { get; set; }
    public string? VehicleModel { get; set; }
    public string? VehicleModelAr { get; set; }
    public string? VehicleColor { get; set; }
    public string? VehicleColorAr { get; set; }
    public string? VehicleNumber { get; set; }
    public List<string>? VehicleImages { get; set; }
    public string? VehicleChassisNumber { get; set; }
    public int? VehicleCapacity { get; set; }
    public int? VehicleYear { get; set; }
    public string? VehicleLicensePlate { get; set; }
    public string? VehicleRegistration { get; set; }
    public decimal? RentalPrice { get; set; }
    public bool? HasDrivers { get; set; }
    public CarBrand? VehicleBrand { get; set; }
    public CarType? CarType { get; set; }
    public ShifterType? ShifterType { get; set; }
    public Guid? WarehouseId { get; set; }
    public List<AttachmentForm>? Attachments { get; set; }
}
