namespace VipTest.reviews.dtos;

public class VehicleReviewDto : ReviewDto
{
    public Guid? VehicleId { get; set; }
    public string? VehicleName { get; set; }
    public string? VehiclePlate { get; set; }   
}