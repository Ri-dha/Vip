using VipTest.AirPortServices.Utli;
using VipTest.Discounts.Models;
using VipTest.Rides.Utli;
using VipTest.Users.customers;
using VipTest.Utlity.Basic;

namespace VipTest.AirPortServices.models;

public class AirportServicesModel : BaseEntity<Guid>
{
    public int? NumberOfCustomers { get; set; }
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    public string? Note { get; set; }
    public string? RejectReason { get; set; }
    public string? CancelReason { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.UnPaid;
    public RidePaymentType PaymentType { get; set; }
    public AirportServicesStatus Status { get; set; } = AirportServicesStatus.Pending;
    public AirportServicesTypes Type { get; set; }
    public decimal Price { get; set; }
    public Guid? DiscountId { get; set; }
    public Discount? Discount { get; set; }
    public decimal? FinalPrice { get; set; }

    public void ApplyDiscount(Discount discount)
    {
        if (discount.IsValidForAirportService(CustomerId, DateTime.Now))
        {
            DiscountId = discount.Id;
            Discount = discount;
            FinalPrice = Price - discount.CalculateDiscount(Price);
            discount.ApplyDiscount(CustomerId);
        }
        else
        {
            FinalPrice = Price;
        }
    }
}