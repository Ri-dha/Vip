using VipTest.FavPlaces.Utli;
using VipTest.Users.customers;
using VipTest.Utlity.Basic;

namespace VipTest.FavPlaces.models;

public class FavouritePlace:BaseEntity<Guid>
{
    public string? PlaceName { get; set; }
    public string? PlaceLocation { get; set; }
    public string? PlaceLocationLatitude { get; set; }
    public string? PlaceLocationLongitude { get; set; }
    public string? PlaceDescription { get; set; }
    public string? PlaceContact { get; set; }
    public PlaceType? PlaceType { get; set; }
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    
}