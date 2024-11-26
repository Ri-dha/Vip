using Microsoft.Extensions.Localization;

namespace VipTest.Localization;

public interface IEnumTranslationService
{
    IEnumerable<EnumTranslation> GetAllEnumTranslations<TEnum>() where TEnum : Enum;
}

public class EnumTranslationService : IEnumTranslationService
{
    private readonly ILocalizationService _localizationService;

    public EnumTranslationService(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    public IEnumerable<EnumTranslation> GetAllEnumTranslations<TEnum>() where TEnum : Enum
    {
        var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        var translations = enumValues.Select(enumValue => new EnumTranslation
        {
            Name = _localizationService.GetLocalizedString($"{typeof(TEnum).Name}_{enumValue}"),
            Value = Convert.ToInt32(enumValue)
        });

        return translations;
    }
}
