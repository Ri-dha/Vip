using VipTest.Utlity.Basic;
using VipTest.vehicles.Modles;

namespace VipTest.attachmentsConfig;

public class Attachments : BaseEntity<Guid>
{
    public string FileName { get; set; }
    public string FilePath { get; set; }

}