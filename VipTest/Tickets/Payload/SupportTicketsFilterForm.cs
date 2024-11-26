using VipTest.Tickets.utli;
using VipTest.Utlity.Basic;

namespace VipTest.Tickets.Payload;

public class SupportTicketsFilterForm:BaseFilter
{
    public Guid? CustomerId { get; set; }
    public Guid? DriverId { get; set; }
    public string? TicketCode { get; set; }
    public TicketType? TicketType { get; set; }
    public TicketStatus? TicketStatus { get; set; }
    public DateTime? ClosedAt { get; set; }
    public Guid? ClosedBy { get; set; }
}