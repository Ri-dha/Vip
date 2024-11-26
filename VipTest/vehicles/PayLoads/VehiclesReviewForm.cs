using System.ComponentModel.DataAnnotations;

namespace VipTest.vehicles.PayLoads;

public class VehiclesReviewForm
{
    [Range(1, 5, ErrorMessage = "Vehicle rating must be between 1 and 5.")]
    public int? VehicleRating { get; set; } 
    
    
    public string? VehicleReview { get; set; }
    
    
}