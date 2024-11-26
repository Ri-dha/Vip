namespace VipTest.reviews.Payloads;

public class VehicleReviewCreateForm
{
    public string? Comment { get; set; }
    public int Rating { get; set; }
    public Guid VehicleId { get; set; }
    
    public Guid? CarRentalOrderId { get; set; }
}