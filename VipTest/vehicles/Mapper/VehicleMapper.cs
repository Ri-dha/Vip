using System.Net.Mail;
using AutoMapper;
using VipTest.attachmentsConfig;
using VipTest.Rides.Dto;
using VipTest.Rides.Models;
using VipTest.vehicles.Dtos;
using VipTest.vehicles.Modles;
using VipTest.vehicles.PayLoads;
using VipTest.vehicles.Utli;

namespace VipTest.vehicles.Mapper;

public class VehicleMapper : Profile
{
    public VehicleMapper()
    {
        // Map Vehicles to VehiclesDto
        CreateMap<Vehicles, VehiclesDto>()
            .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse!.WarehouseName))
            .ForMember(dest => dest.VehicleStatusName, opt => opt.MapFrom(src => src.VehicleStatus.ToString()))
            .ForMember(dest => dest.VehicleTypeName, opt => opt.MapFrom(src => src.VehicleType.ToString()))
            .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments))
            .ForMember(dest => dest.Rides, opt => opt.MapFrom(src => src.Rides))
            .ForMember(dest => dest.VehicleRating, opt => opt.MapFrom(src => src.VehicleRating))
            .ForMember(dest => dest.VehicleImages, opt => opt.MapFrom(src => src.VehicleImages))
            .ReverseMap(); // Map Rides collection
        
        // Map VehiclesDto to Vehicles (only for properties that can be updated via DTO)
        CreateMap<VehiclesDto, Vehicles>()
            .ForMember(dest => dest.Warehouse, opt => opt.Ignore())
            .ForMember(dest => dest.Rides, opt => opt.Ignore())
            .ForMember(dest => dest.Attachments, opt => opt.Ignore()); // Ignore attachments here

        // Map VehicleCreateForm to Vehicles
        CreateMap<VehicleCreateForm, Vehicles>()
            .ForMember(dest => dest.VehicleStatus, opt => opt.MapFrom(src => VehicleStatus.Available))
            .ForMember(dest => dest.VehicleTripCount, opt => opt.Ignore())
            .ForMember(dest => dest.VehicleRating, opt => opt.Ignore())
            .ForMember(dest => dest.VehicleRatingCount, opt => opt.Ignore())
            .ForMember(dest => dest.Warehouse, opt => opt.Ignore())
            .ForMember(dest => dest.Rides, opt => opt.Ignore())
            .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments)); // Map attachments

        // Map VehicleUpdateForm to Vehicles
        CreateMap<VehicleUpdateForm, Vehicles>()
            .ForMember(dest => dest.Rides, opt => opt.Ignore())
            .ForMember(dest => dest.Warehouse, opt => opt.Ignore())
            .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments)) // Map attachments
            .ForMember(dest=>dest.VehicleImages,opt=>opt.MapFrom(src=>src.VehicleImages))
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null)); // Only map non-null properties

        // Map AttachmentForm to Attachment
        CreateMap<AttachmentForm, Attachments>();
        
        CreateMap<Ride, RideDtoForInfo>();
        
        // Map Vehicles to VehiclesDtoForInfo
        CreateMap<Vehicles, VehiclesDtoForInfo>()
            .ForMember(dest => dest.VehicleStatusName, opt => opt.MapFrom(src => src.VehicleStatus.ToString()))
            .ForMember(dest => dest.VehicleTypeName, opt => opt.MapFrom(src => src.VehicleType.ToString()))
            .ForMember(dest => dest.VehicleBrandName, opt => opt.MapFrom(src => src.VehicleBrand.ToString()))
            .ForMember(dest => dest.CarTypeName, opt => opt.MapFrom(src => src.CarType.ToString()))
            .ForMember(dest => dest.ShifterTypeName, opt => opt.MapFrom(src => src.ShifterType.ToString()));

         // Map Vehicles to RentCarsDto
            CreateMap<Vehicles, RentCarsDto>()
                .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse.WarehouseName))
                .ForMember(dest => dest.WarehouseId, opt => opt.MapFrom(src => src.Warehouse.Id))
                .ForMember(dest => dest.WarehouseLocation, opt => opt.MapFrom(src => src.Warehouse.WarehouseLocation))
                .ForMember(dest => dest.WarehouseLocationLatitude, opt => opt.MapFrom(src => src.Warehouse.WarehouseLocationLatitude))
                .ForMember(dest => dest.WarehouseLocationLongitude, opt => opt.MapFrom(src => src.Warehouse.WarehouseLocationLongitude))
                .ForMember(dest => dest.VehicleRating, opt => opt.MapFrom(src => src.VehicleRating))
                .ForMember(dest => dest.VehicleRatingCount, opt => opt.MapFrom(src => src.VehicleRatingCount))
                .ForMember(dest => dest.VehicleTripCount, opt => opt.MapFrom(src => src.VehicleTripCount))
                .ForMember(dest => dest.VehicleCapacity, opt => opt.MapFrom(src => src.VehicleCapacity))
                .ForMember(dest => dest.VehicleYear, opt => opt.MapFrom(src => src.VehicleYear))
                .ForMember(dest => dest.VehicleLicensePlate, opt => opt.MapFrom(src => src.VehicleLicensePlate))
                .ForMember(dest => dest.VehicleRegistration, opt => opt.MapFrom(src => src.VehicleRegistration))
                .ForMember(dest => dest.VehicleStatusName, opt => opt.MapFrom(src => src.VehicleStatus.ToString()))
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments))
                .ForMember(dest => dest.CarRentalOrders, opt => opt.MapFrom(src => src.CarRentalOrders))
                .ForMember(dest => dest.VehicleReviews, opt => opt.MapFrom(src => src.VehicleReviews))
                .ForMember(dest => dest.Warehouse, opt => opt.MapFrom(src => src.Warehouse));

            // Optionally, map RentCarsDto to Vehicles if needed
            CreateMap<RentCarsDto, Vehicles>()
                .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.VehicleType))
                .ForMember(dest => dest.VehicleBrand, opt => opt.MapFrom(src => src.VehicleBrand))
                .ForMember(dest => dest.CarType, opt => opt.MapFrom(src => src.CarType))
                .ForMember(dest => dest.ShifterType, opt => opt.MapFrom(src => src.ShifterType))
                .ForMember(dest => dest.CarAcceptanceStatus, opt => opt.MapFrom(src => src.CarAcceptanceStatus))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))
                .ForMember(dest => dest.WarehouseId, opt => opt.MapFrom(src => src.WarehouseId))
                .ForMember(dest => dest.VehicleStatus, opt => opt.MapFrom(src => src.VehicleStatus))
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments))
                .ForMember(dest => dest.CarRentalOrders, opt => opt.MapFrom(src => src.CarRentalOrders));
      
            // Add mapping between RentCarsCreateForm and Vehicles
            CreateMap<RentCarsCreateForm, Vehicles>()
                .ForMember(dest => dest.VehicleTripCount, opt => opt.Ignore()) // Ignoring fields not part of the form
                .ForMember(dest => dest.VehicleRating, opt => opt.Ignore())
                .ForMember(dest => dest.VehicleRatingCount, opt => opt.Ignore())
                .ForMember(dest => dest.VehicleStatus, opt => opt.MapFrom(src => VehicleStatus.Available)) // Default values
                .ForMember(dest => dest.CarAcceptanceStatus, opt => opt.MapFrom(src => CarAcceptanceStatus.Pending)) // Default values
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments)) // Map attachments
                .ForMember(dest => dest.Rides, opt => opt.Ignore()) // Ignoring fields not part of the form
                .ForMember(dest => dest.Warehouse, opt => opt.Ignore()) // Ignoring warehouse (set separately)
                .ForMember(dest => dest.IsInWarehouse, opt => opt.MapFrom(src => true)) // Set default value
                .ForMember(dest => dest.ForRent, opt => opt.MapFrom(src => true)); // Set default value

            // Add mapping between RentCarsUpdateForm and Vehicles
            CreateMap<RentCarsUpdateForm, Vehicles>()
                .ForMember(dest => dest.VehicleImages, opt => opt.Ignore()) // Ignoring rides
                .ForMember(dest => dest.Attachments, opt => opt.Ignore()) // Ignoring fields not part of the form
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Map only non-null values
            // Only map non-null properties


    }
}