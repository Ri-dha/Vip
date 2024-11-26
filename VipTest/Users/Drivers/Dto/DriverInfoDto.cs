using VipTest.Rentals.Dto;
using VipTest.Rentals.Models;
using VipTest.Rides.Dto;

namespace VipTest.Users.Drivers.Dto;

public class DriverInfoDto
{
    
    public int CompletedRidesPercentage { get; set; }
    public int CanceledRidesPercentage { get; set; }
    public int TotalRides { get; set; }
    public double Rating { get; set; }
    public RideDto LastestRide { get; set; }
    public CarRentalOrderDto LastestCarRentalOrder { get; set; }
    public List<RideDto> RidesForToday { get; set; }
    public List<CarRentalOrderDto> CarRentalOrders { get; set; }
    

}