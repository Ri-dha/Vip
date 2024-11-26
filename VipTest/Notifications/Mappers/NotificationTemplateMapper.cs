using AutoMapper;
using VipTest.Notifications.Dtos;
using VipTest.Notifications.Models;
using VipTest.Notifications.PayLoads;

namespace VipTest.Notifications.Mappers;

public class NotificationTemplateMapper : Profile
{
    public NotificationTemplateMapper()
    {
        // Map NotificationTemplate to NotificationTemplateDto
        CreateMap<NotificationTemplate, NotificationTemplateDto>()
            .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender))
            .ForMember(dest => dest.UserGroups, opt => opt.MapFrom(src => src.UserGroups)) // Map user groups
            .ForMember(dest => dest.SendTo, opt => opt.MapFrom(src => src.SendTo))
            .ReverseMap(); // For two-way mapping

        // Map NotificationTemplateForm to NotificationTemplate
        CreateMap<NotificationTemplateForm, NotificationTemplate>()
            .ForMember(dest => dest.UserGroups, opt => opt.Ignore()) // Handle mapping of user groups separately
            .ForMember(dest => dest.SendTo, opt => opt.Ignore())
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Only map non-null properties

        // Map NotificationTemplateUpdateForm to NotificationTemplate (for updates)
        CreateMap<NotificationTemplateUpdateForm, NotificationTemplate>()
            .ForMember(dest => dest.UserGroups, opt => opt.Ignore()) // Handle user groups update separately
            .ForMember(dest => dest.SendTo, opt => opt.MapFrom(src => src.SendTo))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Only map non-null properties

    }
}