using VipTest.Utlity.Basic;

namespace VipTest.Wallets.PayLoads;

public class WalletFilterForm:BaseFilter
{
    public string? Name { get; set; }
    public Guid? UserId { get; set; }
    public string? Username { get; set; }
    public string? PhoneNumber { get; set; }
    public decimal? BalanceFrom { get; set; }
    public decimal? BalanceTo { get; set; }
}