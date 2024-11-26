using System.Net.Mail;
using VipTest.attachmentsConfig;
using VipTest.Rentals.Models;
using VipTest.reviews.models;
using VipTest.Rides.Models;
using VipTest.Utlity.Basic;
using VipTest.vehicles.Utli;
using VipTest.Warehouses.Models;

namespace VipTest.vehicles.Modles;


public class Vehicles : BaseEntity<Guid>
{
    public string? GeneralDescription { get; set; }
    public string? VehicleName { get; set; }
    public string? VehicleNameAr { get; set; }
    public string? VehicleModel { get; set; }
    public string? VehicleModelAr { get; set; }
    public string? VehicleColor { get; set; }
    public string? VehicleColorAr { get; set; }
    public string? VehicleNumber { get; set; }
    public string? VehicleChassisNumber { get; set; }
    public VehicleType? VehicleType { get; set; }
    public CarBrand? VehicleBrand { get; set; }
    public CarType? CarType { get; set; }
    public ShifterType? ShifterType { get; set; }
    public List<string>? VehicleImages { get; set; }
    public int VehicleTripCount { get; set; } = 0;
    public VehicleStatus VehicleStatus { get; set; } = VehicleStatus.Available;
    public int VehicleRating { get; set; } = 0;
    public int VehicleRatingCount { get; set; } = 0;
    public int? VehicleCapacity { get; set; }
    public int? VehicleYear { get; set; }
    public string? VehicleLicensePlate { get; set; }
    public string? VehicleRegistration { get; set; }

    public bool IsInWarehouse { get; set; }
    public CarAcceptanceStatus? CarAcceptanceStatus { get; set; }

    public string? Note { get; set; }
    public bool? ForRent { get; set; }
    public decimal? RentalPrice { get; set; }
    public decimal? RentalPriceUsd { get; set; }
    
    public bool? HasDrivers { get; set; }

    public Guid? WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }

    public List<Ride> Rides { get; set; } = new List<Ride>();
    public List<CarRentalOrder> CarRentalOrders { get; set; } = new List<CarRentalOrder>();

    public List<VehicleReview> VehicleReviews { get; set; } = new List<VehicleReview>();


    // Add a list of attachments
    public List<Attachments> Attachments { get; set; } = new List<Attachments>();

    public double Rating { get; set; } = 0; // Average rating
    public int TotalRatingCount { get; set; } = 0; // Total number of ratings received
    public double RatingSum { get; set; } = 0; // Sum of all ratings

    // Method to update the rating of the driver when a new review is added
    public void UpdateRating(int newRating)
    {
        // Update the cumulative sum of ratings
        RatingSum += newRating;

        // Increment the total number of ratings received
        TotalRatingCount++;

        // Calculate the new average rating
        Rating = RatingSum / TotalRatingCount;
    }
}