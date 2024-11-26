using VipTest.Rides.Models;

namespace VipTest.reviews.models;

public class RideReview:Review
{
    public Guid RideId { get; set; }
    public Ride Ride { get; set; }
    
}