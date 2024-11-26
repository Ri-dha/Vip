using VipTest.Utlity.Basic;

namespace VipTest.attachmentsConfig;

public class AttachmentDto:BaseDto<Guid>
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
}