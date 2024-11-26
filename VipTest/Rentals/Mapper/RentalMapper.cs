using AutoMapper;
using VipTest.Rentals.Dto;
using VipTest.Rentals.Models;
using VipTest.Rentals.PayLoads;
using VipTest.Rentals.utli;

namespace VipTest.Rentals.Mapper;

public class RentalMapper:Profile
{
    public RentalMapper()
    {
        // Mapping from CarRentalOrder entity to CarRentalOrderDto
        CreateMap<CarRentalOrder, CarRentalOrderDto>()
            .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Driver != null ? src.Driver.Username : null))
            .ForMember(dest => dest.DriverPhone, opt => opt.MapFrom(src => src.Driver != null ? src.Driver.PhoneNumber : null))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Username))
            .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.Customer.Email))
            .ForMember(dest => dest.CustomerPhone, opt => opt.MapFrom(src => src.Customer.PhoneNumber))
            .ForMember(dest => dest.VehicleName, opt => opt.MapFrom(src => src.Vehicle.VehicleName))
            .ForMember(dest => dest.VehiclePlateNumber, opt => opt.MapFrom(src => src.Vehicle.VehicleLicensePlate))
            .ForMember(dest => dest.VehicleColor, opt => opt.MapFrom(src => src.Vehicle.VehicleColor))
            .ForMember(dest => dest.VehicleBrand, opt => opt.MapFrom(src => src.Vehicle.VehicleBrand))
            .ForMember(dest => dest.CarType, opt => opt.MapFrom(src => src.Vehicle.CarType))
            .ForMember(dest => dest.VehicleImages, opt => opt.MapFrom(src => src.Vehicle.VehicleImages))
            .ForMember(dest => dest.ShifterType, opt => opt.MapFrom(src => src.Vehicle.ShifterType))
            .ForMember(dest => dest.VehicleRating, opt => opt.MapFrom(src => src.Vehicle.VehicleRating))
            .ForMember(dest => dest.VehicleCapacity, opt => opt.MapFrom(src => src.Vehicle.VehicleCapacity))
            .ForMember(dest => dest.VehicleYear, opt => opt.MapFrom(src => src.Vehicle.VehicleYear))
            .ForMember(dest => dest.DriverCost, opt => opt.MapFrom(src => src.DriverCost))
            .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments))
            .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Vehicle.Warehouse.WarehouseName))
            .ForMember(dest => dest.IsReviewed, opt => opt.MapFrom(src => src.IsReviewed))
            ;

        // Mapping from CarRentalOrder entity to CarRentalOrderDtoForInfo
        CreateMap<CarRentalOrder, CarRentalOrderDtoForInfo>()
            .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Driver != null ? src.Driver.Username : null))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Username))
            .ForMember(dest => dest.VehicleName, opt => opt.MapFrom(src => src.Vehicle.VehicleName))
            .ForMember(dest => dest.VehiclePlateNumber, opt => opt.MapFrom(src => src.Vehicle.VehicleLicensePlate));

        // Mapping from CarRentalOrderCreateForm to CarRentalOrder entity
        CreateMap<CarRentalOrderCreateForm, CarRentalOrder>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => RentalOrderStatus.Pending)) // Default status
            .ForMember(dest => dest.PickupLocation, opt => opt.MapFrom(src => src.PickupLocation))
            .ForMember(dest => dest.PickupLocationLatitude, opt => opt.MapFrom(src => src.PickupLocationLatitude))
            .ForMember(dest => dest.PickupLocationLongitude, opt => opt.MapFrom(src => src.PickupLocationLongitude))
            .ForMember(dest => dest.DropOffLocation, opt => opt.MapFrom(src => src.DropOffLocation))
            .ForMember(dest => dest.DropOffLocationLatitude, opt => opt.MapFrom(src => src.DropOffLocationLatitude))
            .ForMember(dest => dest.DropOffLocationLongitude, opt => opt.MapFrom(src => src.DropOffLocationLongitude))
            .ForMember(dest => dest.PickupTime, opt => opt.MapFrom(src => src.PickupTime))
            .ForMember(dest => dest.DropOffTime, opt => opt.MapFrom(src => src.DropOffTime));

        // Mapping from CarRentalOrderUpdateForm to CarRentalOrder entity
        CreateMap<CarRentalOrderUpdateForm, CarRentalOrder>()
            .ForMember(dest => dest.PickupLocation, opt => opt.Condition(src => src.PickupLocation != null))
            .ForMember(dest => dest.PickupLocationLatitude, opt => opt.Condition(src => src.PickupLocationLatitude.HasValue))
            .ForMember(dest => dest.PickupLocationLongitude, opt => opt.Condition(src => src.PickupLocationLongitude.HasValue))
            .ForMember(dest => dest.PickupTime, opt => opt.Condition(src => src.PickupTime.HasValue))
            .ForMember(dest => dest.DropOffTime, opt => opt.Condition(src => src.DropOffTime.HasValue))
            .ForMember(dest => dest.Status, opt => opt.Condition(src => src.Status.HasValue));

        // Mapping from CarRentalOrderFilterForm to CarRentalOrder (useful for filtering in queries, etc.)
        CreateMap<CarRentalOrderFilterForm, CarRentalOrder>();
    }
    
}