using VipTest.Discounts.utli;
using VipTest.Rides.Utli;
using VipTest.Users.customers;
using VipTest.Utlity.Basic;
using VipTest.vehicles.Utli;

namespace VipTest.Discounts.Models;

public class Discount : BaseEntity<Guid>
{
    public string Code { get; set; } // Discount code, e.g., "FALL2024"
    public decimal? Amount { get; set; } // Discount amount, used if it's a fixed discount
    public decimal? Percentage { get; set; } // Discount percentage, used if it's a percentage discount
    public decimal? MaxDiscountLimit { get; set; } // Maximum discount amount if percentage is used
    public int? UsageLimitPerUser { get; set; }

    public int UsageCount { get; set; } = 0; // Total usage count across all users
    public bool IsGlobal { get; set; } // Indicates if the discount is global (applies to all users)

    public List<Guid>? ApplicableUserIds { get; set; } = new List<Guid>();

    public DateTimeOffset StartDate { get; set; } // Start date for the discount
    public DateTimeOffset EndDate { get; set; } // Expiration date for the discount

    // Changed to a list of DiscountServices to allow multiple services
    public List<DiscountServices> DiscountServices { get; set; } = new List<DiscountServices>();

    public bool IsPercentage => Percentage.HasValue; // Indicates if the discount is a percentage-based discount

    public List<DiscountUsage> UserUsageCounts { get; set; } = new List<DiscountUsage>();

    // Calculates the effective discount based on the provided original price
    public decimal CalculateDiscount(decimal originalPrice)
    {
        if (IsPercentage && Percentage.HasValue)
        {
            var discountAmount = originalPrice * (Percentage.Value / 100);
            // Apply the maximum discount limit if defined
            return MaxDiscountLimit.HasValue ? Math.Min(discountAmount, MaxDiscountLimit.Value) : discountAmount;
        }
        else if (Amount.HasValue)
        {
            return Amount.Value;
        }

        return 0;
    }

    // Checks if the discount is valid for a given user, date, and ride type
    public bool IsValidForRide(Guid userId, DateTime currentDate, RideType rideType)
    {
        // Check if the discount is expired based on the current date
        if (currentDate < StartDate || currentDate > EndDate) return false;

        // If the discount is not global, check if the user is eligible
        if (!IsGlobal && (ApplicableUserIds == null || !ApplicableUserIds.Contains(userId))) return false;

        // If there's no limit per user (null or 0), the user can use it unlimited times
        if (UsageLimitPerUser == null || UsageLimitPerUser == 0) return true;

        // Check if the user has reached their usage limit per user
        var userUsage = UserUsageCounts.FirstOrDefault(u => u.UserId == userId);
        if (userUsage != null && userUsage.UsageCount >= UsageLimitPerUser) return false;

        // Check if the discount is applicable to the given ride type
        if (!IsDiscountServiceApplicableToRideType(rideType)) return false;

        return true;
    }

    public bool IsValidForCarRent(Guid userId, DateTime currentDate)
    {
        // Check if the discount is expired based on the current date
        if (currentDate < StartDate || currentDate > EndDate) return false;

        // If the discount is not global, check if the user is eligible
        if (!IsGlobal && (ApplicableUserIds == null || !ApplicableUserIds.Contains(userId))) return false;

        // If there's no limit per user (null or 0), the user can use it unlimited times
        if (UsageLimitPerUser == null || UsageLimitPerUser == 0) return true;

        // Check if the user has reached their usage limit per user
        var userUsage = UserUsageCounts.FirstOrDefault(u => u.UserId == userId);
        if (userUsage != null && userUsage.UsageCount >= UsageLimitPerUser) return false;
        if (!IsDiscountServiceApplicableToCarRent()) return false;
        return true;
    }

    public bool IsValidForAirportService(Guid userId, DateTime currentDate)
    {
        // Check if the discount is expired based on the current date
        if (currentDate < StartDate || currentDate > EndDate) return false;

        // If the discount is not global, check if the user is eligible
        if (!IsGlobal && (ApplicableUserIds == null || !ApplicableUserIds.Contains(userId))) return false;

        // If there's no limit per user (null or 0), the user can use it unlimited times
        if (UsageLimitPerUser == null || UsageLimitPerUser == 0) return true;

        // Check if the user has reached their usage limit per user
        var userUsage = UserUsageCounts.FirstOrDefault(u => u.UserId == userId);
        if (userUsage != null && userUsage.UsageCount >= UsageLimitPerUser) return false;
        if (!IsDiscountServiceApplicableToAirportService()) return false;
        return true;
    }
        

    // Method to apply the discount (increments usage count for the user)
    public void ApplyDiscount(Guid userId)
    {
        var userUsage = UserUsageCounts.FirstOrDefault(u => u.UserId == userId);
        if (userUsage != null)
        {
            userUsage.UsageCount++;
        }
        else
        {
            UserUsageCounts.Add(new DiscountUsage { UserId = userId, UsageCount = 1 });
        }

        UsageCount++;
    }

    // Method to check if the discount service is applicable to the ride type
    private bool IsDiscountServiceApplicableToRideType(RideType rideType)
    {
        // This ensures that the discount only applies to rides of specific types that match the services in DiscountService
        return DiscountServices.Any(service =>
            (service == utli.DiscountServices.VipRideService && (rideType == RideType.Vip
                                                                 || rideType == RideType.Vip_To_AlMuthana ||
                                                                 rideType == RideType.Vip_To_Babil
                                                                 || rideType == RideType.Vip_To_DhiQar ||
                                                                 rideType == RideType.Vip_To_Karbala
                                                                 || rideType == RideType.Vip_To_Maysan ||
                                                                 rideType == RideType.Vip_To_Najaf
                                                                 || rideType == RideType.Vip_To_Qadisiyah ||
                                                                 rideType == RideType.Vip_To_Samawa
                                                                 || rideType == RideType.Vip_To_Wasit)) ||
            (service == utli.DiscountServices.NormalRideService && (rideType == RideType.Normal
                                                                    || rideType == RideType.Normal_To_AlMuthana ||
                                                                    rideType == RideType.Normal_To_Babil
                                                                    || rideType == RideType.Normal_To_DhiQar ||
                                                                    rideType == RideType.Normal_To_Karbala
                                                                    || rideType == RideType.Normal_To_Maysan ||
                                                                    rideType == RideType.Normal_To_Najaf
                                                                    || rideType == RideType.Normal_To_Qadisiyah ||
                                                                    rideType == RideType.Normal_To_Samawa
                                                                    || rideType == RideType.Normal_To_Wasit))
        );
    }

    private bool IsDiscountServiceApplicableToCarRent()
    {
        // This ensures that the discount only applies to rides of specific types that match the services in DiscountService
        return DiscountServices.Any(service =>
            (service == utli.DiscountServices.CarRentalService));
    }


    private bool IsDiscountServiceApplicableToAirportService()
    {
        // This ensures that the discount only applies to rides of specific types that match the services in DiscountService
        return DiscountServices.Any(service =>
            (service == utli.DiscountServices.AirportService));
    }
}