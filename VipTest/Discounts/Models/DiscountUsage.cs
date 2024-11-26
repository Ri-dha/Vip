namespace VipTest.Discounts.Models;

public class DiscountUsage
{
    public Guid Id { get; set; } // Primary key
    public Guid UserId { get; set; } // The user who used the discount
    public int UsageCount { get; set; } // How many times the user has used the discount

    // Foreign key to the Discount entity
    public Guid DiscountId { get; set; }
    public Discount Discount { get; set; } // Navigation property to the Discount
}