using VipTest.vehicles.Modles;

namespace VipTest.reviews.models;

public class VehicleReview:Review
{
    public Guid VehicleId { get; set; }
    public Vehicles Vehicles { get; set; }
    
}