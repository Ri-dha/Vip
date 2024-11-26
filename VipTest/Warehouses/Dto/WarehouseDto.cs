using VipTest.attachmentsConfig;
using VipTest.Localization;
using VipTest.Users.Drivers.Dto;
using VipTest.Utlity;
using VipTest.Utlity.Basic;
using VipTest.vehicles.Dtos;

namespace VipTest.Warehouses.Dto;

public class WarehouseDto : BaseDto<Guid>, IDictionaryTranslationSupport
{
    private readonly Dictionary<string, string?> _translatedNames = new();
    Dictionary<string, string?> IDictionaryTranslationSupport.TranslatedNames => _translatedNames;
    public string? ProfileImage { get; set; }
    public string? WarehouseName { get; set; }
    public string? WarehouseNameAr { get; set; }
    public string? WarehouseLocation { get; set; }
    public decimal? WarehouseLocationLatitude { get; set; }
    public decimal? WarehouseLocationLongitude { get; set; }
    public string? WarehouseContact { get; set; }
    public decimal OperationPrecantage { get; set; }

    public string? WarehouseEmail { get; set; }
    public string? WarehousePhone { get; set; }
    public string? WarehouseDescription { get; set; }
    public decimal? DriverCost { get; set; }
    // public bool? HasDrivers { get; set; }
    public bool? IsActive { get; set; }

    public IraqGovernorates? Governorate { get; set; }

    public string? GovernorateName => _translatedNames.TryGetValue(nameof(Governorate), out var value)
        ? value
        : Governorate?.ToString();

    public List<VehiclesDtoForInfo>? WarehouseVehicles { get; set; }
    public int? NumberOfVehicles { get; set; }
    public int? NumberOfAvailableVehicles { get; set; }
    
    public List<AttachmentDto> Attachments { get; set; }

}