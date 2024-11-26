using AutoMapper;
using VipTest.FavPlaces.Dto;
using VipTest.FavPlaces.models;
using VipTest.FavPlaces.Payloads;

namespace VipTest.FavPlaces.Mapper;

public class FavouritePlaceMapper : Profile
{
    public FavouritePlaceMapper()
    {
        // Map FavouritePlace to FavouritePlaceDto
        CreateMap<FavouritePlace, FavouritePlaceDto>()
            .ForMember(dest => dest.PlaceTypeName, opt => opt.MapFrom(src => src.PlaceType.ToString()));

        // Map FavouritePlaceCreateForm to FavouritePlace
        CreateMap<FavouritePlaceCreateForm, FavouritePlace>()
            .ForMember(dest => dest.Customer, opt => opt.Ignore()); // Ignore the Customer navigation property

        // Map FavouritePlaceUpdateForm to FavouritePlace
        CreateMap<FavouritePlaceUpdateForm, FavouritePlace>()
            .ForMember(dest => dest.Customer, opt => opt.Ignore()) // Ignore the Customer navigation property
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null)); // Map only non-null values
    }
}