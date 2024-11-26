using VipTest.Localization;
using VipTest.Users.Admins;
using VipTest.Users.Dtos;
using VipTest.Warehouses.Dto;
using VipTest.Warehouses.Models;

namespace VipTest.Users.BranchManagers;

public class BranchManagerDto : UserDto, IDictionaryTranslationSupport
{
    private readonly Dictionary<string, string?> _translatedNames = new();
    Dictionary<string, string?> IDictionaryTranslationSupport.TranslatedNames => _translatedNames;
    public WarehouseDtoForInfo Warehouse { get; set; }
    public Guid WarehouseId { get; set; }
    public bool isActive { get; set; }

    public AdministrativeRoles? AdministrativeRole { get; set; }

    public string? AdministrativeRoleName => _translatedNames.TryGetValue(nameof(AdministrativeRole), out var value)
        ? value
        : AdministrativeRole?.ToString();
}