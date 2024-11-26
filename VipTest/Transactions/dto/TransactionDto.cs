using System.Transactions;
using VipTest.Localization;
using VipTest.Rentals.Dto;
using VipTest.Rentals.Models;
using VipTest.Rides.Dto;
using VipTest.Rides.Utli;
using VipTest.Transactions.utli;
using VipTest.Utlity.Basic;

namespace VipTest.Transactions.dto;

public class TransactionDto:BaseDto<Guid>, IDictionaryTranslationSupport
{
    private readonly Dictionary<string, string?> _translatedNames = new();
    Dictionary<string, string?> IDictionaryTranslationSupport.TranslatedNames => _translatedNames;
    public string? TransactionCode { get; set; }
    
    public RidePaymentType?  PaymentType { get; set; }
    public string? PaymentTypeName => _translatedNames.TryGetValue(nameof(PaymentType), out var value)
        ? value
        : PaymentType.ToString();
    
    public TransactionPaymentStatus? Status { get; set; }
    public string? StatusName => _translatedNames.TryGetValue(nameof(Status), out var value)
        ? value
        : Status.ToString();
    
    public TransactionsServicesType? ServiceType { get; set; }
    public string? ServiceTypeName => _translatedNames.TryGetValue(nameof(ServiceType), out var value)
        ? value
        : ServiceType.ToString();
    
    public decimal? Amount { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
    public DateTime? CustomerCreatedAt { get; set; }
    public string? CustomerProfilePicture { get; set; }
    
    public RideDto? Ride { get; set; }
    
    public CarRentalOrderDto CarRental { get; set; }
    
}