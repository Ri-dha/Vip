namespace VipTest.Discounts.Payloads;

public class DiscountUpdateForm
{
    public string? Code { get; set; } // Discount code, e.g., "FALL2024"
    
    public decimal? Amount { get; set; } // Discount amount, used if it's a fixed discount

    public decimal? Percentage { get; set; } // Discount percentage, used if it's a percentage discount

    public decimal? MaxDiscountLimit { get; set; } // Maximum discount amount if percentage is used

    public int? UsageLimit { get; set; } // Total times the discount can be used (globally or per user)
    
    public bool? IsGlobal { get; set; } // Indicates if the discount is global (applies to all users)

    public List<Guid>? ApplicableUserIds { get; set; } // List of user IDs that can use this discount if it's not global

    public DateTimeOffset? StartDate { get; set; } // Start date for the discount

    public DateTimeOffset? EndDate { get; set; } // Expiration date for the discount
}