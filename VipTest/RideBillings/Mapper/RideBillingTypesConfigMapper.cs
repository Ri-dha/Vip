using AutoMapper;
using VipTest.RideBillings.Dto;
using VipTest.RideBillings.Models;
using VipTest.RideBillings.Payloads;

namespace VipTest.RideBillings.Mapper;

public class RideBillingTypesConfigMapper:Profile
{
    public RideBillingTypesConfigMapper()
    {
        // Map RideBillingTypesConfig to RideBillingTypesConfigDto
        CreateMap<RideBillingTypesConfig, RideBillingTypesConfigDto>()
            .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IsEnabled));

        // Map RideBillingTypesConfigDto to RideBillingTypesConfig
        CreateMap<RideBillingTypesConfigDto, RideBillingTypesConfig>()
            .ForMember(dest => dest.IsEnabled, opt => opt.Ignore()); // IsEnabled is computed and should not be mapped

        // Map RideBillingTypesConfigCreateForm to RideBillingTypesConfig
        CreateMap<RideBillingTypesConfigCreateForm, RideBillingTypesConfig>()
            .ForMember(dest => dest.RideType, opt => opt.MapFrom(src => src.RideType.Value))
            .ForMember(dest => dest.IsEnabled, opt => opt.Ignore()); // IsEnabled is computed

        // Map RideBillingTypesConfigUpdateForm to RideBillingTypesConfig
        CreateMap<RideBillingTypesConfigUpdateForm, RideBillingTypesConfig>()
            .ForMember(dest => dest.IsEnabled, opt => opt.Ignore()); // IsEnabled is computed
    
        CreateMap<RideBillingTypesConfig,RideBillingSettingsDto>()
            .ForMember(dest => dest.BaseFarePrice, opt => opt.MapFrom(src => src.BaseFarePrice ))
            .ForMember(dest => dest.DetourFarePrice, opt => opt.MapFrom(src => src.DetourFarePrice))
            .ForMember(dest => dest.RideType, opt => opt.MapFrom(src => src.RideType));
    
    }
    
}