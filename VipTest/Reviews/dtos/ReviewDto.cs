using VipTest.reviews.models;
using VipTest.reviews.utli;
using VipTest.Utlity.Basic;

namespace VipTest.reviews.dtos;

public class ReviewDto:BaseDto<Guid>
{
    public string? Comment { get; set; }
    public int? Rating { get; set; }
    
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    
}