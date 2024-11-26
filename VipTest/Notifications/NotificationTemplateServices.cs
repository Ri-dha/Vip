using AutoMapper;
using VipProjectV0._1.Db;
using VipTest.Localization;
using VipTest.Notifications.Dtos;
using VipTest.Notifications.Models;
using VipTest.Notifications.PayLoads;
using VipTest.Notifications.Utli;

namespace VipTest.Notifications;

public interface INotificationTemplateServices
{
    Task<(NotificationTemplateDto? dto, string? error)> Create(NotificationTemplateForm form);
    Task<(NotificationTemplateDto? dto, string? error)> Update(Guid id, NotificationTemplateUpdateForm form);
    Task<(bool, string? error)> Delete(Guid id);
    Task<(NotificationTemplateDto? dto, string? error)> GetById(Guid id);

    Task<(List<NotificationTemplateDto>? dtos, int? totalCount, string? error)> GetAll(
        NotificationTemplateFilterForm filter);
    
    Task SendScheduledNotificationsAsync();

}

public class NotificationTemplateServices : INotificationTemplateServices
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localize;

    public NotificationTemplateServices(IRepositoryWrapper repositoryWrapper, IMapper mapper,
        ILocalizationService localize)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _localize = localize;
    }
    
    private async Task<string> GenerateOrderCode()
    {
        var datePart = DateTime.Now.ToString("yyyyMMdd");
        var latestOrder = await _repositoryWrapper.NotificationTemplateRepository.getLatest(datePart);
        if (latestOrder == null)
        {
            return datePart + "0001";
        }

        var orderCode = latestOrder.NotificationCode;
        var orderNumber = int.Parse(orderCode.Substring(8)) + 1;
        return datePart + orderNumber.ToString().PadLeft(4, '0');
    }


    public async Task<(NotificationTemplateDto? dto, string? error)> Create(NotificationTemplateForm form)
    {
        var notificationTemplate = new NotificationTemplate
        {
            Status = NotificationStatus.Scheduled,
            UserGroups = new List<UserGroups>(),  // Ensure this is initialized
            SendTo = new List<Guid>()             // Ensure this is initialized
        };

        if (form.UserGroupId != null)
        {
            foreach (var groupId in form.UserGroupId)
            {
                var userGroup = await _repositoryWrapper.UserGroupsRepository.Get(x => x.Id == groupId);
                if (userGroup == null)
                {
                    return (null, _localize.GetLocalizedString("UserGroupNotFound"));
                }

                notificationTemplate.UserGroups.Add(userGroup);
                notificationTemplate.SendTo.AddRange(userGroup.userIds);
            }
        }

        if (form.SendTo != null)
        {
            notificationTemplate.SendTo.AddRange(form.SendTo);
        }

        notificationTemplate.NotificationCode = await GenerateOrderCode();
        _mapper.Map(form, notificationTemplate);
        var result = await _repositoryWrapper.NotificationTemplateRepository.Add(notificationTemplate);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToCreateNotificationTemplate"));
        }

        return (_mapper.Map<NotificationTemplateDto>(result), null);
    }


    public async Task<(NotificationTemplateDto? dto, string? error)> Update(Guid id,
        NotificationTemplateUpdateForm form)
    {
        var notificationTemplate = await _repositoryWrapper.NotificationTemplateRepository.Get(x => x.Id == id);
        if (notificationTemplate == null)
        {
            return (null, _localize.GetLocalizedString("NotificationTemplateNotFound"));
        }

        if (form.UserGroupId != null)
        {
            foreach (var groupId in form.UserGroupId)
            {
                if (notificationTemplate.UserGroups.Any(x => x.Id == groupId))
                {
                    continue;
                }

                var userGroup = await _repositoryWrapper.UserGroupsRepository.Get(x => x.Id == groupId);
                if (userGroup == null)
                {
                    return (null, _localize.GetLocalizedString("UserGroupNotFound"));
                }

                notificationTemplate.UserGroups.Add(userGroup);
                notificationTemplate.SendTo.AddRange(userGroup.userIds);
            }
        }

        if (form.SendTo != null)
        {
            foreach (var sendTo in form.SendTo)
            {
                if (notificationTemplate.SendTo.Contains(sendTo))
                {
                    continue;
                }

                notificationTemplate.SendTo.Add(sendTo);
            }
        }

        _mapper.Map(form, notificationTemplate);
        var result = await _repositoryWrapper.NotificationTemplateRepository.Update(notificationTemplate, id);

        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToUpdateNotificationTemplate"));
        }

        return (_mapper.Map<NotificationTemplateDto>(result), null);
    }

    public async Task<(bool, string? error)> Delete(Guid id)
    {
        var result = await _repositoryWrapper.NotificationTemplateRepository.Remove(id);
        if (result == null)
        {
            return (false, _localize.GetLocalizedString("FailedToDeleteNotificationTemplate"));
        }

        return (true, null);
    }

    public async Task<(NotificationTemplateDto? dto, string? error)> GetById(Guid id)
    {
        var result = await _repositoryWrapper.NotificationTemplateRepository.Get(x => x.Id == id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("NotificationTemplateNotFound"));
        }

        return (_mapper.Map<NotificationTemplateDto>(result), null);
    }

    public async Task<(List<NotificationTemplateDto>? dtos, int? totalCount, string? error)> GetAll(
        NotificationTemplateFilterForm filter)
    {
        var (dtos, totalCount) =
            await _repositoryWrapper.NotificationTemplateRepository.GetAll<NotificationTemplateDto>(
                x =>
                    (string.IsNullOrEmpty(filter.Title) || x.Title.Contains(filter.Title)) &&
                    (string.IsNullOrEmpty(filter.NotificationCode) ||
                     x.NotificationCode.Contains(filter.NotificationCode)) &&
                    (filter.Type == null || x.Type == filter.Type) &&
                    (filter.Status == null || x.Status == filter.Status) &&
                    (filter.ScheduledTimeFrom == null || x.ScheduledTime >= filter.ScheduledTimeFrom),
                filter.PageNumber, filter.PageSize);
        return (dtos, totalCount, null);
    }

    public async Task SendScheduledNotificationsAsync()
    {
        var now = DateTime.UtcNow.AddHours(3); // Ensure you're comparing with UTC time

        // Fetch the scheduled notifications
        var (templates, totalCount) = await _repositoryWrapper.NotificationTemplateRepository.GetAll(
            x => x.Status == NotificationStatus.Scheduled && x.ScheduledTime <= now,
            pageNumber: 1,   // Ensure default page number
            pageSize: 30     // Ensure default page size
        );
        // Check if the data part of the tuple contains any results
        if (templates != null && templates.Any())
        {
            foreach (var template in templates)
            {
                template.Status = NotificationStatus.Sent;
                await _repositoryWrapper.NotificationTemplateRepository.Update(template, template.Id);
                
                foreach (var userId in template.SendTo)
                {
                    var userNotification = new UserNotification
                    {
                        ReceiverId = userId,
                        Title = template.Title,
                        Description = template.Description,
                        IsRead = false
                    };
                    await _repositoryWrapper.UserNotificationRepository.Add(userNotification);
                    string[] receiverId = {userId.ToString()};
                OneSignalService.SendNotification(template.Title, template.Description, receiverId);
                }
                
                
            }
        }
    }

}