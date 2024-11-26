using AutoMapper;
using VipTest.reviews.dtos;
using VipTest.reviews.models;

namespace VipTest.reviews.Mappers;

public class ReviewMapper : Profile
{
    public ReviewMapper()
    {
        // Base Review to ReviewDto mapping
        CreateMap<Review, ReviewDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Username : null));

        // DriverReview to DriverReviewDto mapping
        CreateMap<DriverReview, DriverReviewDto>()
            .IncludeBase<Review, ReviewDto>() // Inherit mappings from Review to ReviewDto
            .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Driver.Username));

        // RideReview to RideReviewDto mapping
        CreateMap<RideReview, RideReviewDto>()
            .IncludeBase<Review, ReviewDto>() // Inherit mappings from Review to ReviewDto
            .ForMember(dest => dest.RideCode, opt => opt.MapFrom(src => src.Ride.RidingCode));

        // VehicleReview to VehicleReviewDto mapping
        CreateMap<VehicleReview, VehicleReviewDto>()
            .IncludeBase<Review, ReviewDto>() // Inherit mappings from Review to ReviewDto
            .ForMember(dest => dest.VehicleName, opt => opt.MapFrom(src => src.Vehicles.VehicleName))
            .ForMember(dest => dest.VehiclePlate, opt => opt.MapFrom(src => src.Vehicles.VehicleLicensePlate));
    }
}