namespace VipTest.reviews.Payloads;

public class ReviewCreateForm
{
    public Guid CustomerId { get; set; }
    
    public DriverReviewCreateForm? DriverReview { get; set; }
    public VehicleReviewCreateForm? VehicleReview { get; set; }
    public RideReviewCreateForm? RideReview { get; set; }
}