using VipTest.Rides.Models;
using VipTest.Rides.Utli;
using VipTest.Transactions.utli;
using VipTest.Utlity.Basic;

namespace VipTest.Transactions.dto;

public class TransactionDtoForInfo:BaseDto<Guid>
{
    public string TransactionCode { get; set; }
    public RidePaymentType  PaymentType { get; set; }
    public TransactionPaymentStatus? Status { get; set; }
    public TransactionsServicesType ServiceType { get; set; }
    public decimal Amount { get; set; }
    public string? FromWalletName { get; set; }
    public string? ToWalletName { get; set; }
    public string? RideCode { get; set; }
    public string? CarRentalCode { get; set; }
}