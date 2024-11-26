using VipTest.attachmentsConfig;
using VipTest.Tickets.utli;
using VipTest.Users.customers;
using VipTest.Users.Drivers.Models;
using VipTest.Utlity.Basic;

namespace VipTest.Tickets.models;

public class SupportTickets : BaseEntity<Guid>
{
    public string? TicketCode { get; set; }
    public TicketStatus TicketStatus { get; set; } = TicketStatus.Open;
    public Guid? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public Guid? DriverId { get; set; }
    public Driver? Driver { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<string>? Responses { get; set; }
    public List<Attachments>? Attachments { get; set; }
    public DateTime? ClosedAt { get; set; }
    public Guid? ClosedBy { get; set; }
    public string? ClosedByName { get; set; }
    public Guid? RideCode { get; set; }
    public TicketType TicketType { get; set; }
    
    public Guid? AssignedTo { get; set; }
    public string? AssignedToName { get; set; }
}