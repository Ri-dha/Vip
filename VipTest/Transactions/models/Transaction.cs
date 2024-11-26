using System.Transactions;
using VipTest.Rentals.Models;
using VipTest.Rides.Models;
using VipTest.Rides.Utli;
using VipTest.Transactions.utli;
using VipTest.Users.customers;
using VipTest.Utlity.Basic;
using VipTest.Wallets.Model;

namespace VipTest.Transactions.models;

public class Transaction:BaseEntity<Guid>
{
    public string TransactionCode { get; set; }
    public RidePaymentType  PaymentType { get; set; }
    public TransactionPaymentStatus? Status { get; set; }
    public TransactionsServicesType ServiceType { get; set; }
    public decimal Amount { get; set; }
    
    public Guid? FromWalletId { get; set; }
    public Wallet? FromWallet { get; set; }
    
    public Guid? ToWalletId { get; set; }
    public Wallet? ToWallet { get; set; }
    
    public Guid? RideId { get; set; }
    public Ride? Ride { get; set; }
    public Guid? CarRentalId { get; set; }
    public CarRentalOrder? CarRental { get; set; }
    public Guid? WarehouseId { get; set; }
    
}