using VipTest.Utlity;
using VipTest.Utlity.Basic;

namespace VipTest.Warehouses.Dto;

public class WarehouseDtoForInfo:BaseDto<Guid>
{
    public string? ProfileImage { get; set; }
    public string? WarehouseName { get; set; }
    public string? WarehouseNameAr { get; set; }
    public string? WarehouseLocation { get; set; }
    public decimal? WarehouseLocationLatitude { get; set; }
    public decimal? WarehouseLocationLongitude { get; set; }
    public string? WarehouseContact { get; set; }
    public string? WarehouseEmail { get; set; }
    public string? WarehousePhone { get; set; }
    public string? WarehouseTag { get; set; }
    public string? WarehouseDescription { get; set; }
    public decimal? DriverCost { get; set; }
    public bool? HasDrivers { get; set; }
    public IraqGovernorates? Governorate { get; set; }
    public string? GovernorateName => Governorate.ToString();
    public int? NumberOfVehicles { get; set; }
    public int? NumberOfAvailableVehicles { get; set; }
}