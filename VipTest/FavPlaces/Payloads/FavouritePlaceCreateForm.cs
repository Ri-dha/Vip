﻿using VipTest.FavPlaces.Utli;
using VipTest.Users.customers;
using Swashbuckle.AspNetCore.Annotations; // Required for Swagger annotations

namespace VipTest.FavPlaces.Payloads;

public class FavouritePlaceCreateForm
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
    
    [SwaggerSchema("Customer ID associated with this favorite place (required)")]
    public Guid CustomerId { get; set; }
}