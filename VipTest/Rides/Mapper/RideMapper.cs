using AutoMapper;
using VipTest.Rides.Dto;
using VipTest.Rides.Models;
using VipTest.Rides.Payloads;
using VipTest.Rides.Utli;

namespace VipTest.Rides.Mapper;


public class RideMapper : Profile
{
    public RideMapper()
    {
        // Map Ride to RideDto
        CreateMap<Ride, RideDto>()
            .ForMember(dest => dest.RideCode, opt => opt.MapFrom(src => src.RidingCode))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Username))
            .ForMember(dest => dest.CustomerPhone, opt => opt.MapFrom(src => src.Customer.PhoneNumber))
            .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.Customer.Email))
            .ForMember(dest => dest.CustomerProfilePicture, opt => opt.MapFrom(src => src.Customer.ProfileImage))
            .ForMember(dest => dest.CustomerCreatedAt, opt => opt.MapFrom(src => src.Customer.CreatedAt))
            .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Driver!.Username))
            .ForMember(des=>des.DriverPhone,opt=>opt.MapFrom(src=>src.Driver!.PhoneNumber))
            .ForMember(des=>des.DriverProfilePicture,opt=>opt.MapFrom(src=>src.Driver!.ProfileImage))
            .ForMember(dest => dest.VehicleName, opt => opt.MapFrom(src => src.Vehicle!.VehicleName))
            .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.Vehicle!.VehicleType))
            .ForMember(dest => dest.VehiclePlateNumber, opt => opt.MapFrom(src => src.Vehicle!.VehicleLicensePlate))
            .ForMember(dest => dest.Review, opt => opt.MapFrom(src => src.Review!.Comment))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Review!.Rating))
            .ForMember(dest => dest.VehicleName, opt => opt.MapFrom(src => src.Vehicle.VehicleName))
            .ForMember(dest => dest.VehicleColor, opt => opt.MapFrom(src => src.Vehicle.VehicleColor))
            .ForMember(dest => dest.VehicleBrand, opt => opt.MapFrom(src => src.Vehicle.VehicleBrand))
            .ForMember(dest => dest.CarType, opt => opt.MapFrom(src => src.Vehicle.CarType))
            .ForMember(dest => dest.ShifterType, opt => opt.MapFrom(src => src.Vehicle.ShifterType))
            .ForMember(dest => dest.VehicleRating, opt => opt.MapFrom(src => src.Vehicle.VehicleRating))
            .ForMember(dest => dest.VehicleCapacity, opt => opt.MapFrom(src => src.Vehicle.VehicleCapacity))
            .ForMember(dest => dest.VehicleYear, opt => opt.MapFrom(src => src.Vehicle.VehicleYear))
            .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments));

        // Map RideCreateForm to Ride
        CreateMap<RideCreateForm, Ride>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => RideStatus.Pending)) // Set default status
            .ForMember(dest => dest.Discount, opt => opt.Ignore()) // Discount will be assigned separately
            .ForMember(dest => dest.FinalPrice, opt => opt.Ignore()); // Final price will be calculated

        // Map RideUpdateForm to Ride
        CreateMap<RideUpdateForm, Ride>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Only map non-null properties
   
        CreateMap<Ride,RideDtoForInfo>()
            .ForMember(dest=>dest.DriverId,opt=>opt.MapFrom(src=>src.DriverId))
            .ForMember(dest=>dest.DriverName,opt=>opt.MapFrom(src=>src.Driver!.Username))
            .ForMember(dest=>dest.CustomerId,opt=>opt.MapFrom(src=>src.CustomerId))
            .ForMember(dest=>dest.CustomerName,opt=>opt.MapFrom(src=>src.Customer.Username))
            .ForMember(dest=>dest.FinalPrice,opt=>opt.MapFrom(src=>src.FinalPrice))
            .ForMember(dest=>dest.Status,opt=>opt.MapFrom(src=>src.Status))
            .ForMember(dest=>dest.RideCode,opt=>opt.MapFrom(src=>src.RidingCode))
            .ForMember(dest=>dest.PickupTime,opt=>opt.MapFrom(src=>src.PickupTime));
    }
}