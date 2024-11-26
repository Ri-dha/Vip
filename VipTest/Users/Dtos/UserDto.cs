using VipTest.Localization;
using VipTest.Users.Models;
using VipTest.Utlity;
using VipTest.Utlity.Basic;

namespace VipTest.Users.Dtos;

public class UserDto:BaseDto<Guid>, IDictionaryTranslationSupport
{
    private readonly Dictionary<string, string?> _translatedNames = new();
    Dictionary<string, string?> IDictionaryTranslationSupport.TranslatedNames => _translatedNames;
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string? Email { get; set; }
    public Roles Role { get; set; } 
    public string? RoleName => _translatedNames.TryGetValue(nameof(Role), out var value)
        ? value
        : Role.ToString();
    public string Token { get; set; }
    
    public string? ProfileImage { get; set; }
    
    public DateTime? LastLogin { get; set; }
    
    // get the user's role value
}