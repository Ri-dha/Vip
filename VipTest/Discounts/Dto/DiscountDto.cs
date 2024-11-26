using VipTest.Utlity.Basic;

namespace VipTest.Discounts.Dto;

public class DiscountDto : BaseDto<Guid>
{
    public string? Code { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Percentage { get; set; }
    public decimal? MaxDiscountLimit { get; set; }
    public int? UsageLimit { get; set; } // Limit for each individual user
    public int? UsageCount { get; set; } // Total usage count across all users
    public bool? IsGlobal { get; set; }
    public List<Guid>? ApplicableUserIds { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public bool? IsPercentage { get; set; }
    public bool? IsEnabled { get; set; } // Indicates if the discount is currently active
}