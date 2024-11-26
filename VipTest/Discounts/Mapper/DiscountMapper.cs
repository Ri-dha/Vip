using AutoMapper;
using VipTest.Discounts.Dto;
using VipTest.Discounts.Models;
using VipTest.Discounts.Payloads;

namespace VipTest.Discounts.Mapper;

public class DiscountMapper : Profile
{
    public DiscountMapper()
    {
        CreateMap<Discount, DiscountDto>()
            .ForMember(dest => dest.IsPercentage, opt => opt.MapFrom(src => src.Percentage.HasValue))
            .ForMember(dest => dest.IsEnabled,
                opt => opt.MapFrom(src => src.StartDate <= DateTime.Now && src.EndDate >= DateTime.Now))
            .ForMember(dest => dest.UsageLimit, opt => opt.MapFrom(src => src.UsageLimitPerUser))
            .ForMember(dest => dest.UsageCount, opt => opt.MapFrom(src => src.UsageCount))
            .ForMember(dest => dest.ApplicableUserIds, opt => opt.MapFrom(src => src.ApplicableUserIds))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate));
        
        
        CreateMap<DiscountCreateForm, Discount>()
            .ForMember(dest => dest.UsageCount, opt => opt.MapFrom(src => 0)) // Initialize UsageCount to 0
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Percentage, opt => opt.MapFrom(src => src.Percentage))
            .ForMember(dest => dest.MaxDiscountLimit, opt => opt.MapFrom(src => src.MaxDiscountLimit))
            .ForMember(dest => dest.UsageLimitPerUser, opt => opt.MapFrom(src => src.UsageLimit)) // Map per-user usage limit
            .ForMember(dest => dest.IsGlobal, opt => opt.MapFrom(src => src.IsGlobal))
            .ForMember(dest => dest.ApplicableUserIds, opt => opt.MapFrom(src => src.ApplicableUserIds)) // Reset ApplicableUserIds if IsGlobal
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
            .ForMember(dest => dest.DiscountServices, opt => opt.MapFrom(src => src.DiscountService)); // Map DiscountService

        
        // Map DiscountUpdateForm to Discount
        CreateMap<DiscountUpdateForm, Discount>()
            .ForMember(dest => dest.UsageLimitPerUser,
                opt => opt.MapFrom(src => src.UsageLimit)) // Map per-user usage limit
            .ForMember(dest => dest.ApplicableUserIds, opt => opt.MapFrom(src => src.ApplicableUserIds))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Only map non-null values


        // Map DiscountFilterForm to Discount
        CreateMap<DiscountFilterForm, Discount>()
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
            .ForMember(dest => dest.Percentage,
                opt => opt.Condition(src => src.IsPercentage)); // Filter by percentage if set
    }
}