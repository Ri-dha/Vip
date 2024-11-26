using VipTest.attachmentsConfig;
using VipTest.Utlity;

namespace VipTest.Warehouses.Payloads;

public class WarehouseUpdateForm
{
    public string? ProfileImage { get; set; }
    public string? WarehouseName { get; set; }
    public string? WarehouseLocation { get; set; }
    public decimal? WarehouseLocationLatitude { get; set; }
    public decimal? WarehouseLocationLongitude { get; set; }
    public string? WarehouseContact { get; set; }
    public string? WarehouseEmail { get; set; }
    public bool? IsActive { get; set; }
    public string? WarehousePhone { get; set; }
    public string? WarehouseDescription { get; set; }
    public decimal? DriverCost { get; set; }
    public bool HasDrivers { get; set; }
    public IraqGovernorates? Governorate { get; set; }
    public string? Password { get; set; }
    
    public List<AttachmentForm>? Attachments { get; set; }
    
    public decimal? OperationPrecantage { get; set; }

}