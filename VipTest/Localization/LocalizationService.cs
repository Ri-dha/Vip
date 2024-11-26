using System.Globalization;
using System.Resources;

namespace VipTest.Localization;

public interface ILocalizationService
{
    string GetLocalizedString(string key);
}

public class LocalizationService : ILocalizationService
{
    private readonly ResourceManager _resourceManager;

    public LocalizationService()
    {
        // Replace this with the correct path to your Resources folder and resource base name
        _resourceManager = new ResourceManager("VipTest.Resources.SharedResource", typeof(LocalizationService).Assembly);
    }

    public string GetLocalizedString(string key)
    {
        // Automatically use the current UI culture set by the RequestLocalization middleware
        var cultureInfo = CultureInfo.CurrentUICulture;
        return _resourceManager.GetString(key, cultureInfo) ?? key;  // Fallback to key if no translation is found
    }
}
