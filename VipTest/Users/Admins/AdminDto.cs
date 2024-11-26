using VipTest.Localization;
using VipTest.Users.Dtos;

namespace VipTest.Users.Admins;

public class AdminDto: UserDto, IDictionaryTranslationSupport
{
    private readonly Dictionary<string, string?> _translatedNames = new();
    Dictionary<string, string?> IDictionaryTranslationSupport.TranslatedNames => _translatedNames;
    public string? AdminIdFileUrl { get; set; }  // URL for the ID file
    
    public AdministrativeRoles? AdministrativeRole { get; set; }
    
    public string? AdministrativeRoleName => _translatedNames.TryGetValue(nameof(AdministrativeRole), out var value)
        ? value
        : AdministrativeRole?.ToString();
    
}