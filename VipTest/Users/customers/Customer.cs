using VipTest.FavPlaces.models;
using VipTest.Files.Models;
using VipTest.Rentals.Models;
using VipTest.reviews.models;
using VipTest.Rides.Models;
using VipTest.Users.Models;
using VipTest.vehicles.Modles;

namespace VipTest.Users.customers;

public class Customer : User
{
    public string? IdFile { get; set; } // Navigation property for the ID file

    public string? LicenseFile { get; set; } // Navigation property for the license file

    public bool IsVerified { get; set; } = false;
    public int TripCount { get; set; } = 0;
    public int RentCount { get; set; } = 0;
    public List<Ride> Rides { get; set; } = new List<Ride>();
    public List<Vehicles> FavoriteVehicles { get; set; } = new List<Vehicles>();
    public List<FavouritePlace> FavoritePlaces { get; set; } = new List<FavouritePlace>();
    public CustomerStatus CustomerStatus { get; set; } = CustomerStatus.Active;

    public List<Review> Reviews { get; set; } = new List<Review>();
    public List<CarRentalOrder> CarRentalOrders { get; set; } = new List<CarRentalOrder>();
    public bool IsEnglish { get; set; } = false;
    public bool IsUsd { get; set; } = false;
}