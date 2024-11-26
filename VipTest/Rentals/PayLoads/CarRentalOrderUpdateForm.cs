using Swashbuckle.AspNetCore.Annotations;
using VipTest.Rentals.utli;

namespace VipTest.Rentals.PayLoads;

public class CarRentalOrderUpdateForm
{
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

    [SwaggerSchema("Pickup time for the ride (nullable)")]
    public DateTime? PickupTime { get; set; }

    [SwaggerSchema("Drop-off time for the ride (nullable)")]
    public DateTime? DropOffTime { get; set; }

    [SwaggerSchema("Car back time for the ride (nullable)")]
    public RentalOrderStatus? Status { get; set; }

    [SwaggerSchema("Driver ID associated with this ride (nullable)")]
    public Guid? DriverId { get; set; }
}