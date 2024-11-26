using Swashbuckle.AspNetCore.Annotations;

namespace VipTest.Rides.Payloads;

public class  RideUpdateForm
{
    [SwaggerSchema("Driver ID associated with this ride (nullable)")]
    public Guid? DriverId { get; set; }
    [SwaggerSchema("Driver ID associated with this ride (nullable)")]
    public Guid? VehicleId { get; set; }
    [SwaggerSchema("Pickup location of the ride (nullable)")]
    public string? PickupLocation { get; set; }
    [SwaggerSchema("Latitude of the pickup location (nullable)")]
    public decimal? PickupLocationLatitude { get; set; }
    [SwaggerSchema("Longitude of the pickup location (nullable)")]
    public decimal? PickupLocationLongitude { get; set; }
    [SwaggerSchema("Drop-off location of the ride (nullable)")]
    public string? DropOffLocation { get; set; }
    [SwaggerSchema("Latitude of the drop-off location (nullable)")]
    public decimal? DropOffLocationLatitude { get; set; }
    [SwaggerSchema("Longitude of the drop-off location (nullable)")]
    public decimal? DropOffLocationLongitude { get; set; }
    [SwaggerSchema("Is this ride a detour? (nullable)")]
    public bool? IsDetour { get; set; }
    [SwaggerSchema("Location of the detour (nullable)")]
    public string? DetourLocation { get; set; }
    [SwaggerSchema("Latitude of the detour location (nullable)")]
    public decimal? DetourLocationLatitude { get; set; }
    [SwaggerSchema("Longitude of the detour location (nullable)")]
    public decimal? DetourLocationLongitude { get; set; }
    [SwaggerSchema("Pickup time for the ride (nullable)")]
    public DateTime? PickupTime { get; set; }
    [SwaggerSchema("Discount code for the ride (nullable)")]
    public string? DiscountCode { get; set; }
}