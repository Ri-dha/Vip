using VipTest.Utlity;
using VipTest.Utlity.Basic;

namespace VipTest.Files.Models;

public class ProjectFiles : BaseEntity<Guid>
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public long FileSize { get; set; }
    public string ContentType { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    // Optional properties for additional metadata
    public string? Description { get; set; }
}