using AutoMapper;
using VipProjectV0._1.Db;
using VipTest.FavPlaces.Dto;
using VipTest.FavPlaces.models;
using VipTest.FavPlaces.Payloads;
using VipTest.Localization;

namespace VipTest.FavPlaces;

public interface IFavouritePlaceService
{
    Task<(FavouritePlaceDto? placeDto, string? error)> AddFavouritePlace(FavouritePlaceCreateForm favouritePlace);

    Task<(FavouritePlaceDto? placeDto, string? error)> UpdateFavouritePlace(Guid id,
        FavouritePlaceUpdateForm favouritePlace);

    Task<(FavouritePlaceDto? placeDto, string? error)> GetFavouritePlace(Guid id);
    Task<(List<FavouritePlaceDto>? list,int? totalCount, string? error)> GetFavouritePlacesByCustomerId(Guid customerId);
    Task<(bool success, string? error)> DeleteFavouritePlace(Guid id);
    
}

public class FavouritePlaceService : IFavouritePlaceService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localize;

    public FavouritePlaceService(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILocalizationService localize)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _localize = localize;
    }

    public async Task<(FavouritePlaceDto? placeDto, string? error)> AddFavouritePlace(FavouritePlaceCreateForm favouritePlace)
    {
        
        var customer = await _repositoryWrapper.CustomerRepository.Get(x=>x.Id == favouritePlace.CustomerId);
        if (customer == null)
        {
            return (null, _localize.GetLocalizedString("CustomerNotFound"));
        }
        

        var place = _mapper.Map<FavouritePlace>(favouritePlace);
        place.Customer = customer;
        var addedPlace = await _repositoryWrapper.FavouritePlaceRepository.Add(place);
        if (addedPlace == null)
        {
            return (null, _localize.GetLocalizedString("FavouritePlaceNotAdded"));
        }
        customer.FavoritePlaces.Add(addedPlace);
        await _repositoryWrapper.CustomerRepository.Update(customer, favouritePlace.CustomerId);

        return (_mapper.Map<FavouritePlaceDto>(addedPlace), null);
        
    }

    public async Task<(FavouritePlaceDto? placeDto, string? error)> UpdateFavouritePlace(Guid id, FavouritePlaceUpdateForm favouritePlace)
    {
        
        var place = await _repositoryWrapper.FavouritePlaceRepository.Get(x => x.Id == id);
        if (place == null)
        {
            return (null, _localize.GetLocalizedString("FavouritePlaceNotFound"));
        }

        _mapper.Map(favouritePlace, place);
        var updatedPlace = await _repositoryWrapper.FavouritePlaceRepository.Update(place,id);
        if (updatedPlace == null)
        {
            return (null, _localize.GetLocalizedString("FavouritePlaceNotUpdated"));
        }

        return (_mapper.Map<FavouritePlaceDto>(updatedPlace), null);
        
    }

    public async Task<(FavouritePlaceDto? placeDto, string? error)> GetFavouritePlace(Guid id)
    {
        var place = await _repositoryWrapper.FavouritePlaceRepository.Get(x => x.Id == id);
        if (place == null)
        {
            return (null, _localize.GetLocalizedString("FavouritePlaceNotFound"));
        }

        return (_mapper.Map<FavouritePlaceDto>(place), null);
    }

    public async Task<(List<FavouritePlaceDto>? list, int? totalCount, string? error)> GetFavouritePlacesByCustomerId(Guid customerId)
    {
        var (places, totalCount) = 
            await _repositoryWrapper.FavouritePlaceRepository.GetAll<FavouritePlaceDto>(
            x => x.CustomerId == customerId);
        return (places, totalCount, null);
    }

    public async Task<(bool success, string? error)> DeleteFavouritePlace(Guid id)
    {
        var deletedPlace = await _repositoryWrapper.FavouritePlaceRepository.Remove(id);
        return (true, null);
    }
}