using VipTest.AppSettings.models;
using VipTest.attachmentsConfig;
using VipTest.Discounts.Models;
using VipTest.Rentals.utli;
using VipTest.Rides.Utli;
using VipTest.Users.customers;
using VipTest.Users.Drivers.Models;
using VipTest.Utlity.Basic;
using VipTest.vehicles.Modles;

namespace VipTest.Rentals.Models;

public class CarRentalOrder : BaseEntity<Guid>
{
    public string? OrderCode { get; set; }
    public Guid CustomerId { get; set; }
    public Guid? DriverId { get; set; }
    public Guid VehicleId { get; set; }
    public Customer Customer { get; set; }
    public Driver? Driver { get; set; }
    public Vehicles Vehicle { get; set; }
    public string PickupLocation { get; set; }
    public decimal PickupLocationLatitude { get; set; }
    public decimal PickupLocationLongitude { get; set; }
    
    public string DropOffLocation { get; set; }
    public decimal DropOffLocationLatitude { get; set; }
    public decimal DropOffLocationLongitude { get; set; }
    
    public RentalOrderStatus Status { get; set; }=RentalOrderStatus.Pending;
    public PickUpType PickUpType { get; set; }
    public bool NeedDriver { get; set; }=false; 
    
    public List<Attachments>? Attachments { get; set; } = new List<Attachments>();
    
    public int RentDays { get; set; }
    public int TotalDays { get; set; }=0;
    public Guid? DiscountId { get; set; } // Nullable in case no discount is used
    public Discount? Discount { get; set; } // Navigation property for the applied discount
    
    public decimal DriverCost { get; set; }
    public decimal Price { get; set; }
    public decimal FinalPrice { get; set; } // Price after applying the discount
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
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.UnPaid ;
    public DateTime PickupTime { get; set; }
    public DateTime DropOffTime { get; set; }
    public DateTime? CarBackTime { get; set; }
    public Guid? WarehouseId { get; set; }
    
    public bool IsReviewed { get; set; } = false;
    
    // Method to calculate RentDays and Price
    public void CalculateRentDaysAndPrice(Vehicles vehicle)
    {
        // Calculate the number of rental days
        RentDays = (int)(DropOffTime.Date - PickupTime.Date).TotalDays;

        // If RentDays is less than 1, set it to at least 1
        if (RentDays < 1) RentDays = 1;

        // Calculate the base price (RentalPrice of the vehicle * RentDays)
        Price = vehicle.RentalPrice.HasValue ? vehicle.RentalPrice.Value * RentDays : 0;

        // Check if the vehicle is assigned to a warehouse and if the user needs a driver
        if (NeedDriver && vehicle.Warehouse != null)
        {
            // Add DriverCost from the Warehouse
            Price += vehicle.Warehouse.DriverCost * RentDays;
            DriverCost = vehicle.Warehouse.DriverCost * RentDays;
        }

        // Initially set FinalPrice to the calculated Price
        FinalPrice = Price;
    }


    // Method to apply discount (update FinalPrice)
    public void ApplyDiscount(Discount discount)
    {
        if (discount.IsValidForCarRent(CustomerId, DateTime.Now))
        {
            DiscountId = discount.Id;
            Discount = discount;
            FinalPrice = Price - discount.CalculateDiscount(Price);
            discount.ApplyDiscount(CustomerId);
        }
        else
        {
            FinalPrice = Price; // No discount, FinalPrice is the same as Price
        }
    }
}