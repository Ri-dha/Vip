using VipTest.attachmentsConfig;
using VipTest.Users.BranchManagers;
using VipTest.Users.Drivers.Models;
using VipTest.Users.Models;
using VipTest.Utlity;
using VipTest.Utlity.Basic;
using VipTest.vehicles.Modles;
using VipTest.vehicles.Utli;
using VipTest.Wallets.Model;

namespace VipTest.Warehouses.Models;

public class Warehouse : BaseEntity<Guid>
{
    public string? ProfileImage { get; set; }
    public string? WarehouseName { get; set; }
    public string? WarehouseNameAr { get; set; }
    public string? WarehouseLocation { get; set; }
    public decimal? WarehouseLocationLatitude { get; set; }
    public decimal? WarehouseLocationLongitude { get; set; }
    public string? WarehouseContact { get; set; }
    public string? WarehouseEmail { get; set; }
    public string WarehousePhone { get; set; }
    public string? WarehouseDescription { get; set; }
    public decimal DriverCost { get; set; }=25000;
    public List<Attachments> Attachments { get; set; } = new List<Attachments>();
    
    public decimal OperationPrecantage { get; set; } = 0.1m;

    // public bool? HasDrivers { get; set; }
    public IraqGovernorates? Governorate { get; set; }
    public List<Vehicles>? WarehouseVehicles{ get; set; } = new List<Vehicles>();
    
    
   
    
    public int NumberOfVehicles
    {
        get
        {
            if (WarehouseVehicles != null) return WarehouseVehicles.Count;
            else return 0;
        }

        // set { int NumberOfVehicles = value; }
    }



    public int NumberOfAvailableVehicles
    {
        get
        {
            if (WarehouseVehicles != null)
                return WarehouseVehicles.Count(x => x.VehicleStatus == VehicleStatus.Available);
            else return 0;
        }
        
        // set { int NumberOfAvailableVehicles = value; }
    }

    public Guid BranchManagerId { get; set; }
    public BranchManager BranchManager { get; set; }
}