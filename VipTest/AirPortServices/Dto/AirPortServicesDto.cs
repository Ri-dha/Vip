using VipTest.AirPortServices.Utli;
using VipTest.attachmentsConfig;
using VipTest.Localization;
using VipTest.Rides.Utli;
using VipTest.Utlity.Basic;

namespace VipTest.AirPortServices.Dto;

public class AirPortServicesDto : BaseDto<Guid>, IDictionaryTranslationSupport
{
    private readonly Dictionary<string, string?> _translatedNames = new();
    Dictionary<string, string?> IDictionaryTranslationSupport.TranslatedNames => _translatedNames;
    public int? NumberOfCustomers { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }
    public string? Note { get; set; }
    public PaymentStatus? PaymentStatus { get; set; }

    public string? PaymentStatusName => _translatedNames.TryGetValue(nameof(PaymentStatus), out var value)
        ? value
        : PaymentStatus?.ToString();

    public RidePaymentType? PaymentType { get; set; }

    public string? PaymentTypeName => _translatedNames.TryGetValue(nameof(PaymentType), out var value)
        ? value
        : PaymentType?.ToString();

    public AirportServicesStatus Status { get; set; }

    public string? StatusName => _translatedNames.TryGetValue(nameof(Status), out var value)
        ? value
        : Status.ToString();

    public AirportServicesTypes Type { get; set; }

    public string? TypeName => _translatedNames.TryGetValue(nameof(Type), out var value)
        ? value
        : Type.ToString();

    public decimal Price { get; set; }
    public Guid? DiscountId { get; set; }
    public decimal? FinalPrice { get; set; }
}

public class LoungeServiceDto : AirPortServicesDto
{
}

public class LuggageServiceDto : AirPortServicesDto
{
}

public class VisaVipServiceDto : AirPortServicesDto
{
    public List<AttachmentDto> Attachments { get; set; }
}