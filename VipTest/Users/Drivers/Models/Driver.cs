using VipTest.attachmentsConfig;
using VipTest.Files.Models;
using VipTest.Rentals.Models;
using VipTest.reviews.models;
using VipTest.Rides.Models;
using VipTest.Users.Models;
using VipTest.Warehouses.Models;

namespace VipTest.Users.Drivers.Models;

public class Driver : User
{
    
    public string? userNameAr { get; set; }
    public string? LicenseFile { get; set; } // Navigation property for the driver's license

    public string? IdFile { get; set; } // Navigation property for the ID

    public int TripCount{ get; set; } = 0;  // Number of trips the driver has made
    
    public List<Ride> Rides { get; set; }= new List<Ride>(); // List of rides the driver has made
    public List<CarRentalOrder> CarRentalOrders { get; set; } = new List<CarRentalOrder>(); // List of car rental orders

    public DriverStatus DriverStatus{ get; set; } = DriverStatus.Available;
    public List<Attachments>  Attachments { get; set; } = new List<Attachments>();
    
    public List<DriverReview> DriverReviews { get; set; } = new List<DriverReview>();
    
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