using System.Net.Mail;
using VipTest.attachmentsConfig;
using VipTest.Localization;
using VipTest.Rentals.Dto;
using VipTest.reviews.dtos;
using VipTest.Rides.Dto;
using VipTest.Utlity.Basic;
using VipTest.vehicles.Utli;
using VipTest.Warehouses.Dto;

namespace VipTest.vehicles.Dtos;

public class VehiclesDto : BaseDto<Guid>, IDictionaryTranslationSupport
{
    private readonly Dictionary<string, string?> _translatedNames = new();
    Dictionary<string, string?> IDictionaryTranslationSupport.TranslatedNames => _translatedNames;

    public string? GeneralDescription { get; set; }
    public string? VehicleName { get; set; }
    public string? VehicleNameAr { get; set; }
    public string? VehicleModel { get; set; }
    public string? VehicleModelAr { get; set; }
    public string? VehicleColor { get; set; }
    public string? VehicleColorAr { get; set; }
    public VehicleType? VehicleType { get; set; }

    public string? VehicleTypeName => _translatedNames.TryGetValue(nameof(VehicleType), out var value)
        ? value
        : VehicleType?.ToString();

    public string? VehicleNumber { get; set; }
    public List<string>? VehicleImages { get; set; }
    public string? VehicleChassisNumber { get; set; }
    public int VehicleTripCount { get; set; }
    public int VehicleRating { get; set; }
    public int VehicleRatingCount { get; set; }
    public int? VehicleCapacity { get; set; }
    public int? VehicleYear { get; set; }
    public string? VehicleLicensePlate { get; set; }
    public string? VehicleRegistration { get; set; }

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

    public Guid? WarehouseId { get; set; }
    public string? WarehouseName { get; set; }

    public VehicleStatus VehicleStatus { get; set; }

    public string? VehicleStatusName => _translatedNames.TryGetValue(nameof(VehicleStatus), out var value)
        ? value
        : VehicleStatus.ToString();


    public List<RideDtoForInfo> Rides { get; set; }

    // Add a list of attachments
    public List<Attachments> Attachments { get; set; }
}

public class RentCarsDto : BaseDto<Guid>, IDictionaryTranslationSupport
{
    private readonly Dictionary<string, string?> _translatedNames = new();
    Dictionary<string, string?> IDictionaryTranslationSupport.TranslatedNames => _translatedNames;
    public string? GeneralDescription { get; set; }
    public CarAcceptanceStatus? CarAcceptanceStatus { get; set; }

    public string? CarAcceptanceStatusName => _translatedNames.TryGetValue(nameof(CarAcceptanceStatus), out var value)
        ? value
        : CarAcceptanceStatus?.ToString();

    public string? Note { get; set; }
    public string? VehicleName { get; set; }
    public string? VehicleNameAr { get; set; }
    public string? VehicleModel { get; set; }
    public string? VehicleModelAr { get; set; }
    public string? VehicleColor { get; set; }
    public string? VehicleColorAr { get; set; }
    public VehicleType? VehicleType { get; set; }

    public string? VehicleTypeName => _translatedNames.TryGetValue(nameof(VehicleType), out var value)
        ? value
        : VehicleType?.ToString();

    public string? VehicleNumber { get; set; }
    public List<string>? VehicleImages { get; set; }
    public string? VehicleChassisNumber { get; set; }
    public int VehicleTripCount { get; set; }
    public int VehicleRating { get; set; }
    public int VehicleRatingCount { get; set; }
    public int? VehicleCapacity { get; set; }
    public int? VehicleYear { get; set; }
    public string? VehicleLicensePlate { get; set; }
    public string? VehicleRegistration { get; set; }
    public decimal? RentalPrice { get; set; }
    public decimal? RentalPriceUsd { get; set; }
    public bool? HasDrivers { get; set; }


    public CarBrand? VehicleBrand { get; set; }

    public string? VehicleBrandName => _translatedNames.TryGetValue(nameof(VehicleBrand), out var value)
        ? value
        : VehicleBrand?.ToString();

    public CarType? CarType { get; set; }

    public string? CarTypeName =>
        _translatedNames.TryGetValue(nameof(CarType), out var value) ? value : CarType?.ToString();

    public ShifterType? ShifterType { get; set; }

    public string? ShifterTypeName => _translatedNames.TryGetValue(nameof(ShifterType), out var value)
        ? value
        : ShifterType?.ToString();

    public Guid? WarehouseId { get; set; }
    public string? WarehouseName { get; set; }
    public string? WarehouseLocation { get; set; }
    public decimal? WarehouseLocationLatitude { get; set; }
    public decimal? WarehouseLocationLongitude { get; set; }

    public VehicleStatus VehicleStatus { get; set; }

    public string? VehicleStatusName => _translatedNames.TryGetValue(nameof(VehicleStatus), out var value)
        ? value
        : VehicleStatus.ToString();

    public bool IsInWarehouse { get; set; }

    public WarehouseDtoForInfo Warehouse { get; set; }

    public List<CarRentalOrderDtoForInfo> CarRentalOrders { get; set; }

    // Add a list of attachments
    public List<Attachments> Attachments { get; set; }

    public bool IsFavourite { get; set; } // New property to indicate if the car is a favorite

    public List<VehicleReviewDto> VehicleReviews { get; set; }

    public bool IsReviewed => VehicleReviews.Count != 0; // New property to indicate if the car has been reviewed

    // Only accessible to the DTO itself and for translation purposes
}