using VipTest.vehicles.Dtos;

namespace VipTest.Localization;


public interface IDictionaryTranslationSupport
{
    Dictionary<string, string?> TranslatedNames { get; }
}

public interface IDtoTranslationService
{
    T TranslateEnums<T>(T dto);
}

public class DtoTranslationService : IDtoTranslationService
{
    private readonly IEnumTranslationService _enumTranslationService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DtoTranslationService(IEnumTranslationService enumTranslationService, IHttpContextAccessor httpContextAccessor)
    {
        _enumTranslationService = enumTranslationService;
        _httpContextAccessor = httpContextAccessor;
    }

    public T TranslateEnums<T>(T dto)
    {
        var culture = _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? "en";

        if (dto is IDictionaryTranslationSupport translationSupport)
        {
            foreach (var property in dto.GetType().GetProperties())
            {
                if (property.PropertyType.IsEnum || Nullable.GetUnderlyingType(property.PropertyType)?.IsEnum == true)
                {
                    var enumValue = property.GetValue(dto);
                    if (enumValue != null)
                    {
                        var enumType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        var translation = typeof(IEnumTranslationService)
                            .GetMethod(nameof(IEnumTranslationService.GetAllEnumTranslations))
                            ?.MakeGenericMethod(enumType)
                            .Invoke(_enumTranslationService, null) as IEnumerable<EnumTranslation>;

                        var localizedName = translation?.FirstOrDefault(t => t.Value == Convert.ToInt32(enumValue))?.Name;

                        if (localizedName != null)
                        {
                            translationSupport.TranslatedNames[property.Name] = localizedName;
                        }
                    }
                }
            }
        }

        return dto;
    }


}