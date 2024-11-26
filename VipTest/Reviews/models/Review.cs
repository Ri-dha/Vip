using VipTest.reviews.utli;
using VipTest.Rides.Models;
using VipTest.Users.customers;
using VipTest.Users.Drivers.Models;
using VipTest.Utlity.Basic;

namespace VipTest.reviews.models;

public class Review:BaseEntity<Guid>
{
    
    public string? Comment { get; set; }
    public int? Rating { get; set; }
    
    public ReviewFor ReviewFor { get; set; }
    
    public Guid? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    
    public Guid? DriverCode { get; set; }
    public Guid? VehicleCode { get; set; }
    public Guid? RideCode { get; set; }

    
}