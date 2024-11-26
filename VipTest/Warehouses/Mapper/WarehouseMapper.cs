using AutoMapper;
using VipTest.Warehouses.Dto;
using VipTest.Warehouses.Models;
using VipTest.Warehouses.Payloads;

namespace VipTest.Warehouses.Mapper;

public class WarehouseMapper:Profile
{
    public WarehouseMapper()
    {
         // Mapping between Warehouse and WarehouseDto
            CreateMap<Warehouse, WarehouseDto>()
                .ForMember(dest => dest.NumberOfVehicles, opt => opt.MapFrom(src => src.NumberOfVehicles))
                .ForMember(dest => dest.NumberOfAvailableVehicles, opt => opt.MapFrom(src => src.NumberOfAvailableVehicles))
                .ForMember(dest => dest.WarehouseVehicles, opt => opt.MapFrom(src => src.WarehouseVehicles))
                .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.ProfileImage))
                .ForMember(dest=>dest.IsActive,opt=>opt.MapFrom(src=>src.BranchManager.isActive))
                .ForMember(dest=>dest.Attachments,opt=>opt.MapFrom(src=>src.Attachments))
                ;

            // Mapping between Warehouse and WarehouseDtoForInfo
            CreateMap<Warehouse, WarehouseDtoForInfo>()
                .ForMember(dest => dest.NumberOfVehicles, opt => opt.MapFrom(src => src.NumberOfVehicles))
                .ForMember(dest => dest.NumberOfAvailableVehicles, opt => opt.MapFrom(src => src.NumberOfAvailableVehicles));

            // Mapping between WarehouseCreateForm and Warehouse
            CreateMap<WarehouseCreateForm, Warehouse>()
                .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.WarehouseName))
                .ForMember(dest => dest.WarehouseNameAr, opt => opt.MapFrom(src => src.WarehouseNameAr))
                .ForMember(dest => dest.WarehouseLocation, opt => opt.MapFrom(src => src.WarehouseLocation))
                .ForMember(dest => dest.WarehouseLocationLatitude, opt => opt.MapFrom(src => src.WarehouseLocationLatitude))
                .ForMember(dest => dest.WarehouseLocationLongitude, opt => opt.MapFrom(src => src.WarehouseLocationLongitude))
                .ForMember(dest => dest.WarehouseContact, opt => opt.MapFrom(src => src.WarehouseContact))
                .ForMember(dest => dest.WarehouseEmail, opt => opt.MapFrom(src => src.WarehouseEmail))
                .ForMember(dest => dest.WarehousePhone, opt => opt.MapFrom(src => src.WarehousePhone))
                .ForMember(dest => dest.WarehouseDescription, opt => opt.MapFrom(src => src.WarehouseDescription))
                .ForMember(dest => dest.DriverCost, opt => opt.MapFrom(src => src.DriverCost))
                .ForMember(dest => dest.Governorate, opt => opt.MapFrom(src => src.Governorate))
                .ForMember(dest => dest.OperationPrecantage, opt => opt.MapFrom(src => src.OperationPrecantage))
                .ForMember(dest=>dest.Attachments,opt=>opt.Ignore())
                ;

            // Mapping between WarehouseUpdateForm and Warehouse
            CreateMap<WarehouseUpdateForm, Warehouse>()
                .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.WarehouseName))
                .ForMember(dest => dest.WarehouseLocation, opt => opt.MapFrom(src => src.WarehouseLocation))
                .ForMember(dest => dest.WarehouseLocationLatitude, opt => opt.MapFrom(src => src.WarehouseLocationLatitude))
                .ForMember(dest => dest.WarehouseLocationLongitude, opt => opt.MapFrom(src => src.WarehouseLocationLongitude))
                .ForMember(dest => dest.WarehouseContact, opt => opt.MapFrom(src => src.WarehouseContact))
                .ForMember(dest => dest.WarehouseEmail, opt => opt.MapFrom(src => src.WarehouseEmail))
                .ForMember(dest => dest.WarehousePhone, opt => opt.MapFrom(src => src.WarehousePhone))
                .ForMember(dest => dest.WarehouseDescription, opt => opt.MapFrom(src => src.WarehouseDescription))
                .ForMember(dest => dest.DriverCost, opt => opt.MapFrom(src => src.DriverCost))
                .ForMember(dest => dest.Governorate, opt => opt.MapFrom(src => src.Governorate))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Map only non-null values
    }
}