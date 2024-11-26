using System.Transactions;
using VipTest.Rides.Utli;
using VipTest.Transactions.utli;
using VipTest.Utlity.Basic;

namespace VipTest.Transactions.Payloads;

public class TransactionFilterForm : BaseFilter
{
    public string? TransactionCode { get; set; }
    public RidePaymentType? PaymentType { get; set; }
    public TransactionPaymentStatus? Status { get; set; }

    public List<TransactionsServicesType> ServiceType { get; set; }
    public decimal? Amount { get; set; }
    public Guid? FromWalletId { get; set; }
    public Guid? ToWalletId { get; set; }
    public Guid? WarehouseId { get; set; }
}