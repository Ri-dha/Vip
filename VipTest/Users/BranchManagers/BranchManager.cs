using VipTest.Users.Admins;
using VipTest.Users.Models;
using VipTest.Warehouses.Models;

namespace VipTest.Users.BranchManagers;

public class BranchManager:User
{
    public Guid WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; }
    public bool isActive { get; set; }=true;    
    public AdministrativeRoles? AdministrativeRole { get; set; }

}