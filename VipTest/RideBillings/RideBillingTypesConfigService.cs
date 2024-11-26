using AutoMapper;
using VipProjectV0._1.Db;
using VipTest.Localization;
using VipTest.RideBillings.Dto;
using VipTest.RideBillings.Models;
using VipTest.RideBillings.Payloads;
using VipTest.Rides.Utli;
using VipTest.Utlity.Basic;

namespace VipTest.RideBillings;

public interface IRideBillingTypesConfigService
{
    Task<(RideBillingTypesConfigDto? billingTypesConfigDto,string? error)> CreateAsync(RideBillingTypesConfigCreateForm rideBillingTypesConfigDto);
    Task<(RideBillingTypesConfigDto? billingTypesConfigDto,string? error)> UpdateAsync(Guid id,RideBillingTypesConfigUpdateForm rideBillingTypesConfigDto);
    Task<(RideBillingTypesConfigDto? billingTypesConfigDto,string? error)> DeleteAsync(Guid id);
    Task<(RideBillingTypesConfigDto? billingTypesConfigDto,string? error)> GetByIdAsync(Guid id);
    Task<(List<RideBillingTypesConfigDto>? billingTypesConfigDtos,int? totalCount,string? error)> GetAllAsync(RideBillingTypesConfigFilterForm filter);
    Task<(RideBillingTypesConfigDto? billingTypesConfigDtos,string? error)> GetByRideTypeAsync(RideType rideType);
        
}


public class RideBillingTypesConfigService:IRideBillingTypesConfigService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repo;
    private readonly ILocalizationService _localize;


    public RideBillingTypesConfigService(IMapper mapper, IRepositoryWrapper repo, ILocalizationService localize)
    {
        _mapper = mapper;
        _repo = repo;
        _localize = localize;
    }


    public async Task<(RideBillingTypesConfigDto? billingTypesConfigDto, string? error)> CreateAsync(RideBillingTypesConfigCreateForm rideBillingTypesConfigDto)
    {
        bool isExist = await IsRideBillingTypeExist(rideBillingTypesConfigDto.RideType.Value);
        if (isExist)
        {
            return (null, _localize.GetLocalizedString("RideBillingTypeAlreadyExist"));
        }
        
        var rideBillingTypesConfig = _mapper.Map<RideBillingTypesConfig>(rideBillingTypesConfigDto);
        var result = await _repo.RideBillingRepository.Add(rideBillingTypesConfig);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToCreateRideBilling"));
        }

        return (_mapper.Map<RideBillingTypesConfigDto>(result), null);
        
    }

    public async Task<(RideBillingTypesConfigDto? billingTypesConfigDto, string? error)> UpdateAsync(Guid id,RideBillingTypesConfigUpdateForm rideBillingTypesConfigDto)
    {
        var rideBillingTypesConfig = _mapper.Map<RideBillingTypesConfig>(rideBillingTypesConfigDto);
        var result = await _repo.RideBillingRepository.Update(rideBillingTypesConfig, id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToUpdateRideBilling"));
        }

        return (_mapper.Map<RideBillingTypesConfigDto>(result), null);
    }

    public async Task<(RideBillingTypesConfigDto? billingTypesConfigDto, string? error)> DeleteAsync(Guid id)
    {
        var result = await _repo.RideBillingRepository.Remove(id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToDeleteRideBilling"));
        }

        return (_mapper.Map<RideBillingTypesConfigDto>(result), null);
    }

    public async Task<(RideBillingTypesConfigDto? billingTypesConfigDto, string? error)> GetByIdAsync(Guid id)
    {
        var result = await _repo.RideBillingRepository.GetById(id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToGetRideBilling"));
        }

        return (_mapper.Map<RideBillingTypesConfigDto>(result), null);
    }

    public async Task<(List<RideBillingTypesConfigDto>? billingTypesConfigDtos,int? totalCount, string? error)> GetAllAsync(RideBillingTypesConfigFilterForm filter)
    {
        var (billingTypes, totalCount) = await _repo.RideBillingRepository.GetAll<RideBillingTypesConfigDto>(
            pageNumber: filter.PageNumber, 
            pageSize: filter.PageSize
        );

        return (billingTypes,totalCount, null);
    }

    public async Task<(RideBillingTypesConfigDto? billingTypesConfigDtos, string? error)> GetByRideTypeAsync(RideType rideType)
    {
        var result = await _repo.RideBillingRepository.Get(x => x.RideType == rideType);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToGetRideBilling"));
        }
        return (_mapper.Map<RideBillingTypesConfigDto>(result), null);
    }
    
    private async Task<bool> IsRideBillingTypeExist(RideType rideType)
    {
        var result = await _repo.RideBillingRepository.Get(x => x.RideType == rideType);
        return result != null;
    }
}