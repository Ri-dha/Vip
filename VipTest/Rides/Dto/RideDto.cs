using VipTest.attachmentsConfig;
using VipTest.Localization;
using VipTest.Rides.Utli;
using VipTest.Utlity.Basic;
using VipTest.vehicles.Utli;

namespace VipTest.Rides.Dto;

public class RideDto : BaseDto<Guid>, IDictionaryTranslationSupport
{
    private readonly Dictionary<string, string?> _translatedNames = new();
    Dictionary<string, string?> IDictionaryTranslationSupport.TranslatedNames => _translatedNames;
    public string? RideCode { get; set; }
    public Guid DriverId { get; set; }
    public string? DriverName { get; set; }
    public string? DriverPhone { get; set; }
    public string? DriverProfilePicture { get; set; }
    public Guid CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
    public DateTime? CustomerCreatedAt { get; set; }

    public string? CustomerProfilePicture { get; set; }

    public Guid VehicleId { get; set; }
    public string? VehicleName { get; set; }
    public string? VehiclePlateNumber { get; set; }
    
    public VehicleType? VehicleType { get; set; }
    public string? VehicleTypeName => _translatedNames.TryGetValue(nameof(VehicleType), out var value)
        ? value
        : VehicleType?.ToString();

    public int? VehicleCapacity { get; set; }
    public int? VehicleYear { get; set; }
    public string? VehicleColor { get; set; }
    
    public CarBrand? VehicleBrand { get; set; }

    public string? VehicleBrandName => _translatedNames.TryGetValue(nameof(VehicleBrand), out var value)
        ? value
        : VehicleBrand?.ToString();

    public CarType? CarType { get; set; }

    public string? CarTypeName => _translatedNames.TryGetValue(nameof(CarType), out var value)
        ? value
        : CarType?.ToString();

    public ShifterType? ShifterType { get; set; }
    public string? ShifterTypeName => _translatedNames.TryGetValue(nameof(ShifterType), out var value)
        ? value
        : ShifterType?.ToString();

    public int VehicleRating { get; set; }
    
    public RideType? RideType { get; set; }
    public string? RideTypeName => _translatedNames.TryGetValue(nameof(RideType), out var value)
        ? value
        : RideType?.ToString();

    public Destiantion? RideDestination { get; set; }
    public string? RideDestinationName => _translatedNames.TryGetValue(nameof(RideDestination), out var value)
        ? value
        : RideDestination?.ToString();


    public string? PickupLocation { get; set; }
    public decimal? PickupLocationLatitude { get; set; }
    public decimal? PickupLocationLongitude { get; set; }
    public bool? IsDetour { get; set; }
    public string? DropOffLocation { get; set; }
    public decimal? DropOffLocationLatitude { get; set; }
    public decimal? DropOffLocationLongitude { get; set; }
    public string? DetourLocation { get; set; }
    public decimal? DetourLocationLatitude { get; set; }
    public decimal? DetourLocationLongitude { get; set; }
    public DateTime PickupTime { get; set; }
    public RideStatus Status { get; set; }

    public string? StatusName => _translatedNames.TryGetValue(nameof(Status), out var value)
        ? value
        : Status.ToString();

    public decimal Price { get; set; }

    public bool IsReviewed
    {
        get
        {
            if (Review != null || Rating != null)
            {
                return true;
            }

            return false;
        }
    }

    public string? Review { get; set; }
    public int? Rating { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool? IsCanceledByCustomer { get; set; }
    public string? CancelationReason { get; set; }
    public DateTime? CanceledByCustomerAt { get; set; }
    public DateTime? CanceledByAdminAt { get; set; }
    public Guid? CanceledByAdminId { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime? RejectedByAdminAt { get; set; }
    public Guid? RejectedByAdminId { get; set; }
    public DateTime? AcceptedByAdminAt { get; set; }
    public Guid? AcceptedByAdminId { get; set; }
    public DateTime? StartedByDriverAt { get; set; }
    public RidePaymentType? PaymentType { get; set; }
    public string? PaymentTypeName => _translatedNames.TryGetValue(nameof(PaymentType), out var value)
        ? value
        : PaymentType?.ToString();
    public PaymentStatus? PaymentStatus { get; set; }
    public string? PaymentStatusName => _translatedNames.TryGetValue(nameof(PaymentStatus), out var value)
        ? value
        : PaymentStatus?.ToString();

    public Guid? DiscountId { get; set; }
    public decimal FinalPrice { get; set; }
    public string? DiscountCode { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public decimal? MaxDiscountLimit { get; set; }
    public int? NumberOfPassengers { get; set; }
    public string? CustomerNote { get; set; }
    public bool? VipPassportPackage { get; set; }
    public decimal? VisaCommission { get; set; }
    public bool? MissingBaggagePackage { get; set; }
    public decimal? MissingBaggageCommission { get; set; }

    public List<Attachments>? Attachments { get; set; }
}