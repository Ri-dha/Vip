using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VipTest.Data;
using VipTest.Tickets.models;
using VipTest.Utlity.Basic;

namespace VipTest.Tickets.Repoistory;


public interface ISupportTicketsReposiotry:IBaseRepository<SupportTickets,Guid>
{
        Task<SupportTickets?> GetLatestSupportTicketAsync(string datePart);
    
}

public class SupportTicketsReposiotry:BaseRepository<SupportTickets,Guid>,ISupportTicketsReposiotry
{
        
        private readonly VipProjectContext _db;
    
        public SupportTicketsReposiotry(VipProjectContext context, IMapper mapper) : base(context, mapper)
        { _db = context; }

        public Task<SupportTickets?> GetLatestSupportTicketAsync(string datePart)
        {
                return _db.Set<SupportTickets>()
                        .Where(r => r.TicketCode != null && r.TicketCode.StartsWith(datePart))
                        .OrderByDescending(r => r.TicketCode)
                        .FirstOrDefaultAsync();
        }
}