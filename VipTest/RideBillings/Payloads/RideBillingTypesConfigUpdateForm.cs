namespace VipTest.RideBillings.Payloads;

public class RideBillingTypesConfigUpdateForm
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal BaseFarePrice { get; set; }
    public decimal DetourFarePrice { get; set; }
 }