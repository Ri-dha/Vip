using Swashbuckle.AspNetCore.Annotations;
using VipTest.attachmentsConfig;
using VipTest.Tickets.utli;

namespace VipTest.Tickets.Payload;

public class SupportTicketsCreateForm
{
    [SwaggerSchema("Customer ID associated with this ticket (required)")]
    public Guid? CustomerId { get; set; }
    [SwaggerSchema("Driver ID associated with this ticket (nullable)")]
    public Guid? DriverId { get; set; }
    [SwaggerSchema("Title of the ticket (required)")]
    public string Title { get; set; }
    [SwaggerSchema("Description of the ticket (required)")]
    public string Description { get; set; }
    [SwaggerSchema("Attachemt of the ticket (nullable)")]
    public List<AttachmentForm>? Attachments { get; set; }
    [SwaggerSchema("Type of the ticket (required)")]
    public TicketType TicketType { get; set; }
    
}