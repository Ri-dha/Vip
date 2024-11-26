using VipTest.Utlity.Basic;

namespace VipTest.DesignatedPlaces.model;

public class DesignatedPlaces:BaseEntity<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    
}