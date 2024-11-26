using VipTest.attachmentsConfig;
using VipTest.Localization;
using VipTest.Rentals.Dto;
using VipTest.Rides.Dto;
using VipTest.Users.Dtos;

namespace VipTest.Users.Drivers.Dto;

public class DriverDto : UserDto, IDictionaryTranslationSupport
{
    private readonly Dictionary<string, string?> _translatedNames = new();
    Dictionary<string, string?> IDictionaryTranslationSupport.TranslatedNames => _translatedNames;
    public string? userNameAr { get; set; }

    // Property to store the driver's license file URL
    public string? DriverLicenseUrl { get; set; }

    // Property to store the driver's ID file URL
    public string? DriverIdFileUrl { get; set; }
    
    // Property to store the number of trips the driver has made
    public int TripCount { get; set; }
    
    
    public List<RideDtoForInfo> Rides { get; set; } 
    public List<CarRentalOrderDtoForInfo> CarRentalOrders { get; set; }
    
    public DriverStatus DriverStatus { get; set; }
    public string? DriverStatusName => _translatedNames.TryGetValue(nameof(DriverStatus), out var value)
        ? value
        : DriverStatus.ToString();
    
    // Add a list of attachments
    public List<AttachmentDto> Attachments { get; set; }
    
}