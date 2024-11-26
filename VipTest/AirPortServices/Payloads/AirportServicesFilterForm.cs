using VipTest.AirPortServices.Utli;
using VipTest.Rides.Utli;
using VipTest.Utlity.Basic;

namespace VipTest.AirPortServices.Payloads;

public class AirportServicesFilterForm : BaseFilter
{
    public Guid? CustomerId { get; set; }
    public PaymentStatus? PaymentStatus { get; set; }
    public RidePaymentType? PaymentType { get; set; }
    public AirportServicesStatus? Status { get; set; }
    public AirportServicesTypes? Type { get; set; }
}