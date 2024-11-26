using AutoMapper;
using VipTest.DesignatedPlaces.Dto;
using VipTest.DesignatedPlaces.payloads;

namespace VipTest.DesignatedPlaces.mapper;

public class DesignatedPlacesMapper : Profile
{
    public DesignatedPlacesMapper()
    {
        // Mapping from DesignatedPlaces to DesignatedPlacesDto
        CreateMap<model.DesignatedPlaces, DesignatedPlacesDto>();

        // Mapping from DesignatedPlacesForm to DesignatedPlaces (for creation)
        CreateMap<DesignatedPlacesForm, model.DesignatedPlaces>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude));

        // Mapping from DesignatedPlacesFilterForm to DesignatedPlaces (for filtering, if needed)
        CreateMap<DesignatedPlacesFilterForm, model.DesignatedPlaces>()
            .ForAllMembers(opt => opt.Ignore()); // Ignore for now as there are no properties in the filter form
    }
}