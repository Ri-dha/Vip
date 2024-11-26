using Swashbuckle.AspNetCore.Annotations;
using VipTest.FavPlaces.Utli;

namespace VipTest.FavPlaces.Payloads;

public class FavouritePlaceUpdateForm
{
    [SwaggerSchema("Name of the favorite place (nullable)")]
    public string? PlaceName { get; set; }
    [SwaggerSchema("Location of the favorite place (nullable)")]
    public string? PlaceLocation { get; set; }
    [SwaggerSchema("Latitude of the place location (nullable)")]
    public string? PlaceLocationLatitude { get; set; }
    [SwaggerSchema("Longitude of the place location (nullable)")]
    public string? PlaceLocationLongitude { get; set; }
    [SwaggerSchema("Description of the favorite place (nullable)")]
    public string? PlaceDescription { get; set; }
    [SwaggerSchema("Contact information for the place (nullable)")]
    public string? PlaceContact { get; set; }
    [SwaggerSchema("Type of the place (nullable)")]
    public PlaceType? PlaceType { get; set; }
}