using Swashbuckle.AspNetCore.Annotations;
using VipTest.Tickets.utli;

namespace VipTest.Tickets.Payload;

public class SupportTicketsUpdateForm
{
    
    [SwaggerSchema("Response of the ticket (nullable)")]
    public List<string>? Responses { get; set; }
    
    [SwaggerSchema("Ticket status of the ticket (nullable)")]
    public TicketStatus? TicketStatus { get; set; }
    
}