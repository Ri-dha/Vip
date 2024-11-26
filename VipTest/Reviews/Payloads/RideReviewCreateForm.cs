namespace VipTest.reviews.Payloads;

public class RideReviewCreateForm
{
    public string? Comment { get; set; }
    public int Rating { get; set; }
    public Guid? RideId { get; set; }
}