using AutoMapper;
using VipProjectV0._1.Db;
using VipTest.Localization;
using VipTest.Notifications.Dtos;
using VipTest.Notifications.Models;
using VipTest.Notifications.PayLoads;
using VipTest.Notifications.Utli;

namespace VipTest.Notifications;

public interface IUserNotificationServices
{
    Task<(UserNotificationDto? dto, string? error)> Create(UserNotificationForm form);
    Task<(UserNotificationDto? dto, string? error)> CreateForDriver(UserNotificationForm form);
    Task<(UserNotificationDto? dto, string? error)> Update(Guid id, bool isRead);
    Task<(UserNotificationDto? dto, string? error)> Delete(Guid id);
    Task<(UserNotificationDto? dto, string? error)> GetById(Guid id);
    Task<(List<UserNotificationDto>? dtos, int? totalCount, string? error)> GetAll(UserNotificationFilter filter);
}

public class UserNotificationServices : IUserNotificationServices
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localize;

    public UserNotificationServices(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILocalizationService localize)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _localize = localize;
    }


    public async Task<(UserNotificationDto? dto, string? error)> Create(UserNotificationForm form)
    {
        var userNotification = new UserNotification();

        userNotification.ReceiverId = form.ReceiverId;
        userNotification.Title = form.Title;
        userNotification.Description = form.Description;
        userNotification.IsRead = false;

        var userNotificationResult = await _repositoryWrapper.UserNotificationRepository.Add(userNotification);
        if (userNotificationResult == null)
        {
            return (null, _localize.GetLocalizedString("FailedToCreateUserNotification"));
        }

        string[] userIds = { userNotification.ReceiverId.ToString() };
        OneSignalService.SendNotification(userNotification.Title, userNotification.Description, userIds);
        return (_mapper.Map<UserNotificationDto>(userNotification), null);
    }

    public async Task<(UserNotificationDto? dto, string? error)> CreateForDriver(UserNotificationForm form)
    {
        var userNotification = new UserNotification();

        userNotification.ReceiverId = form.ReceiverId;
        userNotification.Title = form.Title;
        userNotification.Description = form.Description;
        userNotification.IsRead = false;

        var userNotificationResult = await _repositoryWrapper.UserNotificationRepository.Add(userNotification);
        if (userNotificationResult == null)
        {
            return (null, _localize.GetLocalizedString("FailedToCreateUserNotification"));
        }

        string[] userIds = { userNotification.ReceiverId.ToString() };
        OneSignalService.SendNotificationForDriver(userNotification.Title, userNotification.Description, userIds);
        return (_mapper.Map<UserNotificationDto>(userNotification), null);
        
    }

    public async Task<(UserNotificationDto? dto, string? error)> Update(Guid id, bool isRead)
    {
        var userNotification = await _repositoryWrapper.UserNotificationRepository.Get(x => x.Id == id);
        if (userNotification == null)
        {
            return (null, _localize.GetLocalizedString("UserNotificationNotFound"));
        }

        userNotification.IsRead = isRead;
        await _repositoryWrapper.UserNotificationRepository.Update(userNotification, id);
        return (_mapper.Map<UserNotificationDto>(userNotification), null);
    }

    public async Task<(UserNotificationDto? dto, string? error)> Delete(Guid id)
    {
        var userNotification = await _repositoryWrapper.UserNotificationRepository.Get(x => x.Id == id);
        if (userNotification == null)
        {
            return (null, _localize.GetLocalizedString("UserNotificationNotFound"));
        }

        await _repositoryWrapper.UserNotificationRepository.Remove(id);
        return (_mapper.Map<UserNotificationDto>(userNotification), null);
    }

    public async Task<(UserNotificationDto? dto, string? error)> GetById(Guid id)
    {
        var userNotification = await _repositoryWrapper.UserNotificationRepository.Get(x => x.Id == id);
        if (userNotification == null)
        {
            return (null, _localize.GetLocalizedString("UserNotificationNotFound"));
        }

        return (_mapper.Map<UserNotificationDto>(userNotification), null);
    }

    public async Task<(List<UserNotificationDto>? dtos, int? totalCount, string? error)> GetAll(
        UserNotificationFilter filter)
    {
        var (dtos, totalCount) =
            await _repositoryWrapper.UserNotificationRepository.GetAll<UserNotificationDto>(
                x =>
                    (filter.ReceiverId == null || x.ReceiverId == filter.ReceiverId) &&
                    (filter.IsRead == null || x.IsRead == filter.IsRead) &&
                    (filter.DateTimeFrom == null || x.CreatedAt >= filter.DateTimeFrom) &&
                    (filter.DateTimeTo == null || x.CreatedAt <= filter.DateTimeTo),
                
                filter.PageNumber, filter.PageSize);
        return (dtos, totalCount, null);
    }
}