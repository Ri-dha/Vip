using VipTest.Transactions.dto;
using VipTest.Utlity.Basic;

namespace VipTest.Wallets.Dtos;

public class WalletDto : BaseDto<Guid>
{
    public string? Name { get; set; }
    public decimal Balance { get; set; } = 0;
    public decimal TotalIncome { get; set; } = 0;
    public decimal TotalExpense { get; set; } = 0;
    public List<TransactionDtoForInfo> Transactions { get; set; }
}