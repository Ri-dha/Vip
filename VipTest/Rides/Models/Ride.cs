using VipTest.attachmentsConfig;
using VipTest.Discounts.Models;
using VipTest.reviews.models;
using VipTest.Rides.Utli;
using VipTest.Users.customers;
using VipTest.Users.Drivers.Models;
using VipTest.Users.Models;
using VipTest.Utlity.Basic;
using VipTest.vehicles.Modles;
using VipTest.vehicles.Utli;

namespace VipTest.Rides.Models;

public class Ride : BaseEntity<Guid>
{
    public string? RidingCode { get; set; }
    public Guid CustomerId { get; set; }
    public Guid? DriverId { get; set; }
    public Guid? VehicleId { get; set; }
    public string PickupLocation { get; set; }
    public decimal PickupLocationLatitude { get; set; }
    public decimal PickupLocationLongitude { get; set; }
    public string DropOffLocation { get; set; }
    public decimal DropOffLocationLatitude { get; set; }
    public decimal DropOffLocationLongitude { get; set; }
    public bool? IsDetour { get; set; }
    public string? DetourLocation { get; set; }
    public decimal? DetourLocationLatitude { get; set; }
    public decimal? DetourLocationLongitude { get; set; }
    public DateTime? PickupTime { get; set; }
    public RideType RideType { get; set; }
    public Destiantion RideDestination { get; set; }
    public RideStatus Status { get; set; }
    public decimal Price { get; set; }

    // New properties for Discount integration
    public Guid? DiscountId { get; set; } // Nullable in case no discount is used
    public Discount? Discount { get; set; } // Navigation property for the applied discount
    public decimal FinalPrice { get; set; } // Price after applying the discount

    public RideReview? Review { get; set; }
    public DateTime? CompletedAt { get; set; }
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
    public RidePaymentType? PaymentType { get; set; }

    public PaymentStatus PaymentStatus = PaymentStatus.UnPaid;

    public Customer Customer { get; set; }
    public Driver? Driver { get; set; }
    public Vehicles? Vehicle { get; set; }


    public bool? WelcomePackage { get; set; }
    public decimal? WelcomePackageCommission { get; set; }
    public int? NumberOfBags { get; set; }
    public int? NumberOfPassengers { get; set; }
    public string? CustomerNote { get; set; }
    public bool? VipPassportPackage { get; set; }
    public decimal? VisaCommission { get; set; }

    public bool? MissingBaggagePackage { get; set; }

    public decimal? MissingBaggageCommission { get; set; }

    // Add a list of attachments
    public List<Attachments>? Attachments { get; set; } = new List<Attachments>();

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

    // Method to apply a discount and calculate the final price
    public void ApplyDiscount(Discount discount)
    {
        if (discount.IsValidForRide(CustomerId, DateTime.Now, RideType))
        {
            DiscountId = discount.Id;
            Discount = discount;
            FinalPrice = Price - discount.CalculateDiscount(Price);
            discount.ApplyDiscount(CustomerId); // Increment the usage count of the discount
        }
        else
        {
            FinalPrice = Price; // No discount applied, keep the original price
        }
    }
}