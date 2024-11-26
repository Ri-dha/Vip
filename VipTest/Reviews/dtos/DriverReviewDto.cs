namespace VipTest.reviews.dtos;

public class DriverReviewDto: ReviewDto
{
    public Guid? DriverId { get; set; }
    public string? DriverName { get; set; }
 
    
}