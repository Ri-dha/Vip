using VipTest.Utlity;
using VipTest.Utlity.Basic;

namespace VipTest.Warehouses.Payloads;

public class WarehouseFilterForm : BaseFilter
{
    public string? WarehouseName { get; set; }
    public string? WarehouseLocation { get; set; }
    // public string? WarehouseLocationLatitude { get; set; }
    // public string? WarehouseLocationLongitude { get; set; }
    // public string? RadiusKm { get; set; }
    public bool? IsActive { get; set; }
    public IraqGovernorates? Governorate { get; set; }
}