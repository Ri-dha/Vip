namespace VipTest.Utlity.Basic;

public class BaseDto<TId>
{
    public TId Id { get; set; }

    public bool Deleted { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}