using Swashbuckle.AspNetCore.Annotations;
using VipTest.attachmentsConfig;
using VipTest.Rentals.utli;

namespace VipTest.Rentals.PayLoads;

public class CarRentalOrderCreateForm
{
    [SwaggerSchema("Customer Id(Required)")]
    public Guid CustomerId { get; set; }

    [SwaggerSchema("Vehicle Id(Required)")]
    public Guid VehicleId { get; set; }

    [SwaggerSchema("Pickup Location(Required)")]
    public string PickupLocation { get; set; }

    [SwaggerSchema("Pickup Location Latitude(Required)")]
    public decimal PickupLocationLatitude { get; set; }

    [SwaggerSchema("Pickup Location Longitude(Required)")]
    public decimal PickupLocationLongitude { get; set; }

    [SwaggerSchema("DropOff Location(Required)")]
    public string DropOffLocation { get; set; }

    [SwaggerSchema("DropOff Location Latitude(Required)")]

    public decimal DropOffLocationLatitude { get; set; }

    [SwaggerSchema("DropOff Location Longitude(Required)")]

    public decimal DropOffLocationLongitude { get; set; }

    [SwaggerSchema("Pickup Type(Required)")]
    public PickUpType PickUpType { get; set; }


    [SwaggerSchema("Discount code for the ride (nullable)")]
    public string? DiscountCode { get; set; }

    [SwaggerSchema("PickupTime (Required)")]
    public DateTime PickupTime { get; set; }

    [SwaggerSchema("DropOffTime(Required)")]
    public DateTime DropOffTime { get; set; }

    [SwaggerSchema("Need Driver(Required)")]
    public bool NeedDriver { get; set; }

    [SwaggerSchema("To add passport for the ride (nullable)")]
    public List<AttachmentForm>? Attachments { get; set; }
}