using VipTest.Discounts.utli;
using VipTest.Utlity.Basic;

namespace VipTest.Discounts.Payloads;

public class DiscountFilterForm : BaseFilter
{
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public bool IsPercentage { get; set; }

    public List<DiscountServices>? Services { get; set; } // List of services to filter by
}