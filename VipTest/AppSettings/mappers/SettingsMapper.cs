using AutoMapper;
using VipTest.AppSettings.Dto;
using VipTest.AppSettings.models;
using VipTest.AppSettings.Payloads;
using VipTest.RideBillings.Dto;
using VipTest.RideBillings.Models;

namespace VipTest.AppSettings.Mapper
{
    public class SettingsMapper : Profile
    {
        public SettingsMapper()
        {
            // Map Settings to SettingsDto
            CreateMap<Settings, SettingsDto>()
                .ForMember(dest => dest.VipPrice, opt => opt.Ignore())
                .ForMember(dest => dest.VipDetourPrice, opt => opt.Ignore())
                .ForMember(dest => dest.NormalPrice, opt => opt.Ignore())
                .ForMember(dest => dest.NormalDetourPrice, opt => opt.Ignore()); // This is handled separately

            // Map SettingsDto to Settings
            CreateMap<SettingsDto, Settings>()
                .ForMember(dest => dest.RideBillingTypesConfigs, opt => opt.Ignore()); // This is handled separately

            // Map SettingsUpdateForm to Settings
            CreateMap<SettingsUpdateForm, Settings>()
                .ForMember(dest => dest.RideBillingTypesConfigs, opt => opt.Ignore()); // This is handled separately

            // Map RideBillingTypesConfig to RideBillingSettingsDto
            CreateMap<RideBillingTypesConfig, RideBillingSettingsDto>()
                .ForMember(dest => dest.RideType, opt => opt.MapFrom(src => src.RideType))
                .ForMember(dest => dest.BaseFarePrice, opt => opt.MapFrom(src => src.BaseFarePrice))
                .ForMember(dest => dest.DetourFarePrice, opt => opt.MapFrom(src => src.DetourFarePrice));

            // Map RideBillingSettingsDto to RideBillingTypesConfig
            CreateMap<RideBillingSettingsDto, RideBillingTypesConfig>()
                .ForMember(dest => dest.RideType, opt => opt.MapFrom(src => src.RideType))
                .ForMember(dest => dest.BaseFarePrice, opt => opt.MapFrom(src => src.BaseFarePrice))
                .ForMember(dest => dest.DetourFarePrice, opt => opt.MapFrom(src => src.DetourFarePrice));
        }
    }
}