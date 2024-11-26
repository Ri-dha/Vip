using AutoMapper;
using VipTest.Notifications.Dtos;
using VipTest.Notifications.Models;
using VipTest.Notifications.PayLoads;

namespace VipTest.Notifications.Mappers;

public class UserGroupsMapper: Profile
{
    public UserGroupsMapper()
    {
        // Map UserGroups to UserGroupsDto
        CreateMap<UserGroups, UserGroupsDto>()
            .ForMember(dest => dest.userIds, opt => opt.MapFrom(src => src.userIds))
            .ReverseMap();

        // Map UserGroups to UserGroupsDtoForInfo (basic info)
        CreateMap<UserGroups, UserGroupsDtoForInfo>()
            .ReverseMap();

        // Map UserGroupsForm to UserGroups
        CreateMap<UserGroupsForm, UserGroups>()
            .ForMember(dest => dest.userIds, opt => opt.MapFrom(src => src.userIds))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Only map non-null properties

        // Map UserGroupsUpdateForm to UserGroups
        CreateMap<UserGroupsUpdateForm, UserGroups>()
            .ForMember(dest => dest.userIds, opt => opt.MapFrom(src => src.userIds))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Only map non-null properties
    }
}