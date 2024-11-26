using AutoMapper;
using VipTest.AirPortServices.Dto;
using VipTest.AirPortServices.models;
using VipTest.AirPortServices.Payloads;

namespace VipTest.AirPortServices.Mapper;

public class AirportServicesMapper:Profile
{
    
    public AirportServicesMapper()
    {
        // General AirportServices Mapping
        CreateMap<AirportServicesModel, AirPortServicesDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Username))
            .ForMember(dest => dest.CustomerPhone, opt => opt.MapFrom(src => src.Customer.PhoneNumber))
            ;

        CreateMap<AirportServicesCreateForm, AirportServicesModel>()
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Discount, opt => opt.Ignore());

        CreateMap<AirportServicesUpdateForm, AirportServicesModel>()
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Discount, opt => opt.Ignore());

        // Lounge Service Mapping
        CreateMap<LoungeService, LoungeServiceDto>()
            .IncludeBase<AirportServicesModel, AirPortServicesDto>();

        CreateMap<LoungeServiceCreateForm, LoungeService>()
            .IncludeBase<AirportServicesCreateForm, AirportServicesModel>();

        CreateMap<LoungeUpdateForm, LoungeService>()
            .IncludeBase<AirportServicesUpdateForm, AirportServicesModel>();

        // Luggage Service Mapping
        CreateMap<LuggageService, LuggageServiceDto>()
            .IncludeBase<AirportServicesModel, AirPortServicesDto>();

        CreateMap<LuggageServiceCreateForm, LuggageService>()
            .IncludeBase<AirportServicesCreateForm, AirportServicesModel>();

        CreateMap<LuggageUpdateForm, LuggageService>()
            .IncludeBase<AirportServicesUpdateForm, AirportServicesModel>();

        // Visa VIP Service Mapping
        CreateMap<VisaVipService, VisaVipServiceDto>()
            .IncludeBase<AirportServicesModel, AirPortServicesDto>()
            .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments));

        CreateMap<VisaVipServiceCreateForm, VisaVipService>()
            .ForMember(dest => dest.Attachments, opt => opt.Ignore())
            .IncludeBase<AirportServicesCreateForm, AirportServicesModel>();

        CreateMap<VisaVipUpdateForm, VisaVipService>()
            .IncludeBase<AirportServicesUpdateForm, AirportServicesModel>()
            .ForMember(dest => dest.Attachments, opt => opt.Ignore());

    }
    
}