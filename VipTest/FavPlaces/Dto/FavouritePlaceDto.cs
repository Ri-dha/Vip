using VipTest.FavPlaces.Utli;
using VipTest.Users.customers;
using VipTest.Utlity.Basic;

namespace VipTest.FavPlaces.Dto;

public class FavouritePlaceDto:BaseDto<Guid>
{
    public string? PlaceName { get; set; }
    public string? PlaceLocation { get; set; }
    public string? PlaceLocationLatitude { get; set; }
    public string? PlaceLocationLongitude { get; set; }
    public string? PlaceDescription { get; set; }
    public string? PlaceContact { get; set; }
    public PlaceType? PlaceType { get; set; }
    public string? PlaceTypeName => PlaceType.ToString();
    public Guid CustomerId { get; set; }
}