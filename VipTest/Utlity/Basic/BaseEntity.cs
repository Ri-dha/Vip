using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VipTest.Utlity.Basic;

public class BaseEntity<TId>
{
    [Key]
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public TId Id { get; set; }

    public bool Deleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}