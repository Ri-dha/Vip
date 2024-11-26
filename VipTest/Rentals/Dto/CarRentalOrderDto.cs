using VipTest.attachmentsConfig;
using VipTest.Localization;
using VipTest.Rentals.utli;
using VipTest.Rides.Utli;
using VipTest.Utlity.Basic;
using VipTest.vehicles.Utli;

namespace VipTest.Rentals.Dto;

public class CarRentalOrderDto : BaseDto<Guid>, IDictionaryTranslationSupport
{
    private readonly Dictionary<string, string?> _translatedNames = new();
    Dictionary<string, string?> IDictionaryTranslationSupport.TranslatedNames => _translatedNames;

    public string? OrderCode { get; set; }
    public Guid DriverId { get; set; }
    public string? DriverName { get; set; }
    public string? DriverPhone { get; set; }
    public Guid CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
    public DateTime? CustomerCreatedAt { get; set; }
    public string? CustomerProfilePicture { get; set; }
    public Guid VehicleId { get; set; }
    public string? VehicleName { get; set; }
    public string? VehiclePlateNumber { get; set; }
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

    public List<string>? VehicleImages { get; set; }

    public ShifterType? ShifterType { get; set; }

    public string? ShifterTypeName => _translatedNames.TryGetValue(nameof(ShifterType), out var value)
        ? value
        : ShifterType?.ToString();

    public int? VehicleRating { get; set; }
    public string? PickupLocation { get; set; }
    public decimal? PickupLocationLatitude { get; set; }
    public decimal? PickupLocationLongitude { get; set; }
    public string? DropOffLocation { get; set; }
    public decimal? DropOffLocationLatitude { get; set; }
    public decimal? DropOffLocationLongitude { get; set; }
    public bool? IsCanceledByCustomer { get; set; }
    public string? CancelationReason { get; set; }
    public DateTime? CanceledByCustomerAt { get; set; }
    public DateTime? CanceledByAdminAt { get; set; }
    public Guid? CanceledByAdminId { get; set; }
    public DateTime? RejectedByAdminAt { get; set; }
    public string? RejectionReason { get; set; }
    public Guid? RejectedByAdminId { get; set; }
    public DateTime? AcceptedByAdminAt { get; set; }
    public Guid? AcceptedByAdminId { get; set; }
    public DateTime? StartedByDriverAt { get; set; }
    public DateTime PickupTime { get; set; }
    public DateTime DropOffTime { get; set; }
    public DateTime? CarBackTime { get; set; }
    public RidePaymentType? PaymentType { get; set; }

    public string? PaymentTypeName => _translatedNames.TryGetValue(nameof(PaymentType), out var value)
        ? value
        : PaymentType?.ToString();

    public RentalOrderStatus? Status { get; set; }
    
    public string? StatusName => _translatedNames.TryGetValue(nameof(Status), out var value)
        ? value
        : Status?.ToString();

    public PickUpType? PickUpType { get; set; }

    public string? PickUpTypeName => _translatedNames.TryGetValue(nameof(PickUpType), out var value)
        ? value
        : PickUpType?.ToString();

    public bool? NeedDriver { get; set; }
    public List<Attachments>? Attachments { get; set; }
    public int RentDays { get; set; }
    public int? TotalDays { get; set; }
    public string? WarehouseName { get; set; }

    public bool IsReviewed { get; set; }


    public PaymentStatus? PaymentStatus { get; set; }

    public string? PaymentStatusName => _translatedNames.TryGetValue(nameof(PaymentStatus), out var value)
        ? value
        : PaymentStatus?.ToString();

    public decimal Price { get; set; }
    public decimal? DriverCost { get; set; }
    public Guid? DiscountId { get; set; }
    public decimal FinalPrice { get; set; }
    public string? DiscountCode { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public decimal? MaxDiscountLimit { get; set; }
}