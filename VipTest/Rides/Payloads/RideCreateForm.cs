using Swashbuckle.AspNetCore.Annotations;
using VipTest.attachmentsConfig;
using VipTest.Rides.Utli;

namespace VipTest.Rides.Payloads;

public class RideCreateForm
{
    [SwaggerSchema("Customer ID associated with this ride (required)")]
    public Guid CustomerId { get; set; }
    [SwaggerSchema("Driver ID associated with this ride (required)")]
    public string PickupLocation { get; set; }
    [SwaggerSchema("Latitude of the pickup location (required)")]
    public decimal PickupLocationLatitude { get; set; }
    [SwaggerSchema("Longitude of the pickup location (required)")]
    public decimal PickupLocationLongitude { get; set; }
    [SwaggerSchema("Drop-off location of the ride (required)")]
    public string DropOffLocation { get; set; }
    [SwaggerSchema("Latitude of the drop-off location (required)")]
    public decimal DropOffLocationLatitude { get; set; }
    [SwaggerSchema("Longitude of the drop-off location (required)")]
    public decimal DropOffLocationLongitude { get; set; }
    [SwaggerSchema("Is this ride a detour? (nullable)")]
    public bool? IsDetour { get; set; }
    [SwaggerSchema("Location of the detour (nullable)")]
    public string? DetourLocation { get; set; }
    [SwaggerSchema("Latitude of the detour location (nullable)")]
    public decimal? DetourLocationLatitude { get; set; }
    [SwaggerSchema("Longitude of the detour location (nullable)")]
    public decimal? DetourLocationLongitude { get; set; }
    [SwaggerSchema("Pickup time for the ride (required)")]
    public DateTime PickupTime { get; set; }
    [SwaggerSchema("Type of the ride (required)")]
    public RideType RideType { get; set; }
    
    [SwaggerSchema("Destination of the ride (required)")]
    public Destiantion RideDestination { get; set; }
    [SwaggerSchema("Discount code for the ride (nullable)")]
    public string? DiscountCode { get; set; }
    [SwaggerSchema("Review for the ride (nullable)")]
    public RidePaymentType? PaymentType { get; set; }
    
    [SwaggerSchema("To add passport for the ride (required)")]
    public bool? VipPassportPackage { get; set; }
    [SwaggerSchema("Attachments for the ride (nullable)")]
    public List<AttachmentForm>? Attachments { get; set; }
    
    [SwaggerSchema("Number of passengers for the ride (nullable)")]
    public int? NumberOfPassengers { get; set; }
    [SwaggerSchema("Customer note for the ride (nullable)")]
    public string? CustomerNote { get; set; }

    [SwaggerSchema("To add missing baggage package for the ride (required)")]
    public bool? MissingBaggagePackage { get; set; }

    [SwaggerSchema("To add welcome package for the ride (required)")]
    public bool? WelcomePackage { get; set; }
}