using AutoMapper;
using VipProjectV0._1.Db;
using VipTest.Localization;
using VipTest.Notifications.Dtos;
using VipTest.Notifications.Models;
using VipTest.Notifications.PayLoads;

namespace VipTest.Notifications;


public interface IUserGroupsServices
{
    Task<(UserGroupsDto? dto,string? error)> Create(UserGroupsForm form);
    Task<(UserGroupsDto? dto,string? error)> Update(Guid id, UserGroupsUpdateForm form);
    Task<(UserGroupsDto? dto,string? error)> Delete(Guid id);
    Task<(UserGroupsDto? dto,string? error)> GetById(Guid id);
    Task<(List<UserGroupsDto>? dtos,int? totalCount,string? error)> GetAll(UserGroupsFilter filter);
}

public class UserGroupsServices:IUserGroupsServices
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localize;

    public UserGroupsServices(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILocalizationService localize)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _localize = localize;
    }


    public async Task<(UserGroupsDto? dto, string? error)> Create(UserGroupsForm form)
    {
        
        var userGroups = _mapper.Map<UserGroups>(form);
        var result = await _repositoryWrapper.UserGroupsRepository.Add(userGroups);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToCreateUserGroups"));
        }
        
        return (_mapper.Map<UserGroupsDto>(result), null);
    }

    public async Task<(UserGroupsDto? dto, string? error)> Update(Guid id, UserGroupsUpdateForm form)
    {
        var group = await _repositoryWrapper.UserGroupsRepository.Get(x=>x.Id==id);
        if (group == null)
        {
            return (null, _localize.GetLocalizedString("UserGroupsNotFound"));
        }

        _mapper.Map(form, group);
        var result = await _repositoryWrapper.UserGroupsRepository.Update(group, id);
        
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToUpdateUserGroups"));
        }
        
        return (_mapper.Map<UserGroupsDto>(result), null);
    }

    public async Task<(UserGroupsDto? dto, string? error)> Delete(Guid id)
    {
        var userGroups = await _repositoryWrapper.UserGroupsRepository.Get(x=>x.Id==id);
        
        if (userGroups.code=="1"||userGroups.code=="2"||userGroups.code=="3")
        {
            return (null, _localize.GetLocalizedString("CanNotDeleteDefaultUserGroups"));
        }
        
        var result = await _repositoryWrapper.UserGroupsRepository.Remove(id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToDeleteUserGroups"));
        }
        return (_mapper.Map<UserGroupsDto>(result), null);
        
    }

    public async Task<(UserGroupsDto? dto, string? error)> GetById(Guid id)
    {
        var result = await _repositoryWrapper.UserGroupsRepository.Get(x=>x.Id==id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("UserGroupsNotFound"));
        }
        return (_mapper.Map<UserGroupsDto>(result), null);
    }

    public async Task<(List<UserGroupsDto>? dtos, int? totalCount, string? error)> GetAll(UserGroupsFilter filter)
    {
        
        var (userGroups, totalCount) = await _repositoryWrapper.UserGroupsRepository.GetAll<UserGroupsDto>
            (
            x =>
                string.IsNullOrEmpty(filter.Title) || x.Title.Contains(filter.Title)
            ,filter.PageNumber, filter.PageSize);
        return (userGroups, totalCount, null);
    }
}