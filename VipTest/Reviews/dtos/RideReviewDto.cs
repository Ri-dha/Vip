namespace VipTest.reviews.dtos;

public class RideReviewDto: ReviewDto
{
    
    public Guid? RideId { get; set; }
    public string? RideCode { get; set; }
    
}