namespace VipTest.reviews.Payloads;

public class DriverReviewCreateForm
{
    public string? Comment { get; set; }
    public int Rating { get; set; }
    public Guid DriverId { get; set; }
}