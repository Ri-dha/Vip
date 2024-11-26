using AutoMapper;
using VipTest.Notifications.Dtos;
using VipTest.Notifications.Models;
using VipTest.Notifications.PayLoads;

namespace VipTest.Notifications.Mappers;

public class UserNotificationMapper : Profile
{
    public UserNotificationMapper()
    {
        // Map UserNotification to UserNotificationDto and vice versa
        CreateMap<UserNotification, UserNotificationDto>()
            .ReverseMap(); // Reverse mapping for cases where you need to map DTO back to entity
        
        // Map UserNotificationForm to UserNotification (used for creating new notifications)
        CreateMap<UserNotificationForm, UserNotification>();
    }
}