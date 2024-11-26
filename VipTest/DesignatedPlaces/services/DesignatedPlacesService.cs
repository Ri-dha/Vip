using AutoMapper;
using VipProjectV0._1.Db;
using VipTest.DesignatedPlaces.Dto;
using VipTest.DesignatedPlaces.payloads;
using VipTest.Localization;

namespace VipTest.DesignatedPlaces.services;
public interface IDesignatedPlacesService
{
    Task<(DesignatedPlacesDto? placesDto,string? error)> CreateDesignatedPlaces(DesignatedPlacesForm form);
    Task<(DesignatedPlacesDto? placesDto,string? error)> UpdateDesignatedPlaces(Guid id, DesignatedPlacesForm form);
    Task<(DesignatedPlacesDto? placesDto,string? error)> GetDesignatedPlacesById(Guid id);
    Task<(List<DesignatedPlacesDto>? placesDtos,int? totalCount,string? error)> GetAllDesignatedPlaces(DesignatedPlacesFilterForm filterForm);
    Task<(bool,string? error)> DeleteDesignatedPlaces(Guid id);
    
}


public class DesignatedPlacesService:IDesignatedPlacesService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localize;

    public DesignatedPlacesService(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILocalizationService localize)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _localize = localize;
    }


    public async Task<(DesignatedPlacesDto? placesDto, string? error)> CreateDesignatedPlaces(DesignatedPlacesForm form)
    {
        
        var designatedPlaces = _mapper.Map<model.DesignatedPlaces>(form);
        var result = await _repositoryWrapper.DesignatedPlacesRepository.Add(designatedPlaces);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToCreateDesignatedPlaces"));
        }

        return (_mapper.Map<DesignatedPlacesDto>(result), null);
        
    }

    public async Task<(DesignatedPlacesDto? placesDto, string? error)> UpdateDesignatedPlaces(Guid id, DesignatedPlacesForm form)
    {
        
        var designatedPlaces = _mapper.Map<model.DesignatedPlaces>(form);
        designatedPlaces.Id = id;
        var result = await _repositoryWrapper.DesignatedPlacesRepository.Update(designatedPlaces, id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToUpdateDesignatedPlaces"));
        }

        return (_mapper.Map<DesignatedPlacesDto>(result), null);
        
    }

    public async Task<(DesignatedPlacesDto? placesDto, string? error)> GetDesignatedPlacesById(Guid id)
    {
        var result = await _repositoryWrapper.DesignatedPlacesRepository.GetById(id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("DesignatedPlacesNotFound"));
        }

        return (_mapper.Map<DesignatedPlacesDto>(result), null);
    }

    public async Task<(List<DesignatedPlacesDto>? placesDtos, int? totalCount, string? error)> GetAllDesignatedPlaces(DesignatedPlacesFilterForm filterForm)
    {
        var result = await _repositoryWrapper.DesignatedPlacesRepository.GetAll<DesignatedPlacesDto>(
            filterForm.PageNumber, filterForm.PageSize
            );

        return (_mapper.Map<List<DesignatedPlacesDto>>(result.data), result.totalCount, null);
    }

    public async Task<(bool, string? error)> DeleteDesignatedPlaces(Guid id)
    {
        var result = await _repositoryWrapper.DesignatedPlacesRepository.Remove(id);
        if (result==null)
        {
            return (false, _localize.GetLocalizedString("DesignatedPlacesNotFound"));
        }
        
        return (true, null);
    }
}