using AutoMapper;
using VipTest.Tickets.Dto;
using VipTest.Tickets.models;
using VipTest.Tickets.Payload;
using VipTest.Users.Drivers.Dto;
using VipTest.Users.Drivers.Models;

namespace VipTest.Tickets.Mapper;

public class SupportTicketsMapper:Profile
{
    public SupportTicketsMapper()
    {
           // Mapping from SupportTickets to SupportTicketsDto
            CreateMap<SupportTickets, SupportTicketsDto>()
                .ForMember(dest => dest.TicketTypeName, opt => opt.MapFrom(src => src.TicketType.ToString()))
                .ForMember(dest => dest.TicketStatusName, opt => opt.MapFrom(src => src.TicketStatus.ToString()))
                .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssignedTo))
                .ForMember(dest => dest.AssignedToName, opt => opt.MapFrom(src => src.AssignedToName))
                
                // Map Driver to DriverInfoDto
                .ForMember(dest => dest.Driver, opt => opt.MapFrom(src => src.Driver));

            // Mapping from SupportTicketsCreateForm to SupportTickets
            CreateMap<SupportTicketsCreateForm, SupportTickets>()
                .ForMember(dest => dest.Attachments, opt => opt.Ignore()) // Assuming Attachments are handled separately
                .ForMember(dest => dest.TicketStatus, opt => opt.Ignore()) // TicketStatus is set to default
                .ForMember(dest => dest.TicketCode, opt => opt.Ignore()) // Assuming TicketCode is generated separately
                .ForMember(dest => dest.ClosedAt, opt => opt.Ignore()) // Not relevant during creation
                .ForMember(dest => dest.ClosedBy, opt => opt.Ignore()) // Not relevant during creation
                .ForMember(dest => dest.ClosedByName, opt => opt.Ignore()) // Not relevant during creation
                .ForMember(dest => dest.AssignedTo, opt => opt.Ignore()) // AssignedTo is not set during creation
                .ForMember(dest => dest.AssignedToName, opt => opt.Ignore()); // AssignedToName is not set during creation

            // Mapping from SupportTicketsUpdateForm to SupportTickets
            CreateMap<SupportTicketsUpdateForm, SupportTickets>()
                .ForMember(dest => dest.Responses, opt => opt.Condition((src, dest, srcMember) => srcMember != null)) // Map only if Responses is not null
                .ForMember(dest => dest.TicketStatus, opt => opt.MapFrom(src => src.TicketStatus))
                .ForMember(dest => dest.Attachments, opt => opt.Ignore()) // Assuming Attachments are not updated here
                .ForMember(dest => dest.TicketCode, opt => opt.Ignore()) // Do not update TicketCode
                .ForMember(dest => dest.ClosedAt, opt => opt.Ignore()) // ClosedAt is updated separately
                .ForMember(dest => dest.ClosedBy, opt => opt.Ignore()) // ClosedBy is updated separately
                .ForMember(dest => dest.ClosedByName, opt => opt.Ignore()) // ClosedByName is updated separately
                .ForMember(dest => dest.AssignedTo, opt => opt.Ignore()) // AssignedTo is updated separately
                .ForMember(dest => dest.AssignedToName, opt => opt.Ignore()); // AssignedToName is updated separately

            // Mapping for Driver -> DriverInfoDto (needed for SupportTickets to SupportTicketsDto)
            CreateMap<Driver, DriverInfoDto>(); }
    
}