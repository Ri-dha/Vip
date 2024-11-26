using VipTest.FavPlaces.Dto;
using VipTest.Localization;
using VipTest.Rentals.Dto;
using VipTest.Rentals.Models;
using VipTest.Rides.Dto;
using VipTest.Users.Dtos;
using VipTest.vehicles.Dtos;

namespace VipTest.Users.customers;

public class CustomerDto : UserDto, IDictionaryTranslationSupport
{
    private readonly Dictionary<string, string?> _translatedNames = new();
    Dictionary<string, string?> IDictionaryTranslationSupport.TranslatedNames => _translatedNames;
    
    public string? CustomerIdFileUrl { get; set; } // URL for the ID file
    public string? CustomerLicenseFileUrl { get; set; } // URL for the license file
    public bool IsVerified { get; set; } // Indicates if the customer is verified
    
    public int TripCount { get; set; } // Number of trips the customer has made
    public int RentCount{ get; set; } 

    public List<RideDtoForInfo> Rides { get; set; } // List of rides the customer has made
    public List<CarRentalOrderDtoForInfo> CarRentalOrders { get; set; } // List of car rental orders

    public List<VehiclesDtoForInfo> FavoriteVehicles { get; set; } // List of favorite vehicles

    public List<FavouritePlaceDto> FavoritePlaces { get; set; } // List of favorite places
    
    public CustomerStatus CustomerStatus { get; set; } // Customer status
    public string? CustomerStatusName => _translatedNames.TryGetValue(nameof(CustomerStatus), out var value)
        ? value
        : CustomerStatus.ToString();
    public bool IsUsd { get; set; }
}