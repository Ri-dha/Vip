using VipTest.Users.Drivers.Models;

namespace VipTest.reviews.models;

public class DriverReview:Review
{
    public Guid DriverId { get; set; }
    public Driver Driver { get; set; }
}