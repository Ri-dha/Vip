using VipTest.Localization;
using VipTest.Tickets.utli;
using VipTest.Users.customers;
using VipTest.Users.Drivers.Dto;
using VipTest.Utlity.Basic;

namespace VipTest.Tickets.Dto;

public class SupportTicketsDto:BaseDto<Guid>, IDictionaryTranslationSupport
{
    private readonly Dictionary<string, string?> _translatedNames = new();
    Dictionary<string, string?> IDictionaryTranslationSupport.TranslatedNames => _translatedNames;
    
    public string? TicketCode { get; set; }
    public Guid CustomerId { get; set; }
    public CustomerDto Customer { get; set; }
    public Guid? DriverId { get; set; }
    public DriverInfoDto? Driver { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<string>? Responses { get; set; }
    public DateTime? ClosedAt { get; set; }
    public Guid? ClosedBy { get; set; }
    public string? ClosedByName { get; set; }
    public Guid? RideCode { get; set; }
    public TicketType TicketType { get; set; }
    public string? TicketTypeName=> _translatedNames.TryGetValue(nameof(TicketType), out var value)
        ? value
        : TicketType.ToString();
    public TicketStatus TicketStatus{get;set;}
    public string? TicketStatusName=> _translatedNames.TryGetValue(nameof(TicketStatus), out var value)
        ? value
        : TicketStatus.ToString();
    
    public Guid? AssignedTo { get; set; }
    public string? AssignedToName { get; set; }
    
}