using AutoMapper;
using VipTest.attachmentsConfig;
using VipTest.Users.Admins;
using VipTest.Users.BranchManagers;
using VipTest.Users.customers;
using VipTest.Users.Drivers.Dto;
using VipTest.Users.Drivers.Models;
using VipTest.Users.Drivers.PayLoads;
using VipTest.Users.Dtos;
using VipTest.Users.Models;
using VipTest.Users.OTP;
using VipTest.Users.PayLoad;

namespace VipTest.Users.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.ProfileImage));
        CreateMap<UserDto, User>();
        CreateMap<UserForm, User>();
        CreateMap<User, User>().ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<UserUpdateForm, User>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Driver Mappings
        CreateMap<Driver, DriverDto>()
            .IncludeBase<User, UserDto>() // Inherit base mappings
            .ForMember(dest => dest.DriverLicenseUrl, opt => opt.MapFrom(src => src.LicenseFile))
            .ForMember(dest => dest.userNameAr, opt => opt.MapFrom(src => src.userNameAr))
            .ForMember(dest => dest.DriverIdFileUrl, opt => opt.MapFrom(src => src.IdFile))
            .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments))
            .ForMember(dest=> dest.Rides , opt=>opt.MapFrom(src=> src.Rides)  )
            .ForMember(dest => dest.DriverStatus, opt => opt.MapFrom(src => src.DriverStatus)); // Include attachments

        CreateMap<DriverForm, Driver>()
            .IncludeBase<UserForm, User>() // Inherit mappings from UserForm to User
            .ForMember(dest => dest.LicenseFile, opt => opt.MapFrom(src => src.DriverLicense))
            .ForMember(dest => dest.IdFile, opt => opt.MapFrom(src => src.DriverIdFile))
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.userNameAr, opt => opt.MapFrom(src => src.userNameAr))
            .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments)); // Map attachments

        CreateMap<DriverUpdateForm, Driver>()
            .IncludeBase<UserUpdateForm, User>() // Inherit mappings from UserUpdateForm to User
            .ForMember(dest => dest.LicenseFile, opt => opt.MapFrom(src => src.DriverLicenseFile))
            .ForMember(dest => dest.IdFile, opt => opt.MapFrom(src => src.DriverIdFile))
            .ForMember(dest => dest.Password, opt => opt.Ignore()) // Ignore password mapping
            .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments))
            .ForMember(dest => dest.DriverStatus, opt => opt.MapFrom(src => src.DriverStatus))
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null)); // Map only non-null values

        // Attachments Mappings
        CreateMap<AttachmentForm, Attachments>(); // Map AttachmentForm to Attachments
        CreateMap<Attachments, AttachmentDto>(); // Map Attachments to AttachmentDto
        // Customer Mappings
        CreateMap<Customer, CustomerDto>()
            .IncludeBase<User, UserDto>() // Inherit base mappings
            .ForMember(dest => dest.CustomerIdFileUrl, opt => opt.MapFrom(src => src.IdFile))
            .ForMember(dest => dest.CustomerLicenseFileUrl, opt => opt.MapFrom(src => src.LicenseFile))
            .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => src.IsVerified))
            .ForMember(dest => dest.FavoritePlaces, opt => opt.MapFrom(src => src.FavoritePlaces))
            .ForMember(dest => dest.CustomerStatus, opt => opt.MapFrom(src => src.CustomerStatus)) // Map CustomerStatus
            .ForMember(dest=> dest.Rides,opt=> opt.MapFrom((src=>src.Rides)))
            .ForMember(dest=> dest.CarRentalOrders,opt=> opt.MapFrom(src=>src.CarRentalOrders))
            .ForMember(dest=> dest.FavoriteVehicles,opt=> opt.MapFrom(src=>src.FavoriteVehicles))
            .ForMember(dest=> dest.FavoritePlaces,opt=> opt.MapFrom(src=>src.FavoritePlaces)); // Map CustomerStatusName

        CreateMap<CustomerForm, Customer>()
            .IncludeBase<UserForm, User>() // Inherit mappings from UserForm to User
            .ForMember(dest => dest.IdFile, opt => opt.MapFrom(src => src.CustomerIdFile))
            .ForMember(dest => dest.LicenseFile, opt => opt.MapFrom(src => src.CustomerLicenseFile))
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.IsVerified, opt => opt.Ignore())
            .ForMember(dest => dest.CustomerStatus,
                opt => opt.Ignore()); // Ignore CustomerStatus on creation, it will be set to Active by default

        CreateMap<CustomerUpdateForm, Customer>()
            .IncludeBase<UserUpdateForm, User>() // Inherit mappings from UserUpdateForm to User
            .ForMember(dest => dest.IdFile, opt => opt.MapFrom(src => src.CustomerIdFile))
            .ForMember(dest => dest.LicenseFile, opt => opt.MapFrom(src => src.CustomerLicenseFile))
            .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => src.IsVerified))
            .ForMember(dest => dest.CustomerStatus, opt => opt.MapFrom(src => src.CustomerStatus)) // Map CustomerStatus
            .ForMember(dest=> dest.IsEnglish, opt=> opt.MapFrom(src=> src.IsEnglish))
            .ForMember(dest=> dest.IsUsd, opt=> opt.MapFrom(src=> src.IsUsd))
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null)); // Map only non-null values

        // Admin Mappings
        CreateMap<Admin, AdminDto>()
            .IncludeBase<User, UserDto>() // Inherit base mappings
            .ForMember(dest => dest.AdminIdFileUrl, opt => opt.MapFrom(src => src.AdminIdFile))
            .ForMember(dest => dest.AdministrativeRole, opt => opt.MapFrom(src => src.AdministrativeRole))
            .ForMember(dest => dest.AdministrativeRoleName, opt => opt.MapFrom(src => src.AdministrativeRole));

        CreateMap<AdminForm, Admin>()
            .IncludeBase<UserForm, User>() // Inherit mappings from UserForm to User
            .ForMember(dest => dest.AdminIdFile, opt => opt.MapFrom(src => src.AdminIdFile))
            .ForMember(dest => dest.AdministrativeRole, opt => opt.MapFrom(src => src.AdministrativeRole))
            .ForMember(dest => dest.Password, opt => opt.Ignore());

        CreateMap<AdminUpdateForm, Admin>()
            .IncludeBase<UserUpdateForm, User>() // Inherit mappings from UserUpdateForm to User
            .ForMember(dest => dest.AdminIdFile, opt => opt.MapFrom(src => src.AdminIdFile))
            .ForMember(dest => dest.AdministrativeRole, opt => opt.MapFrom(src => src.AdministrativeRole))
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null)); // Map only non-null values

        
        // Branch Manager Mappings
        CreateMap<BranchManager, BranchManagerDto>()
            .IncludeBase<User, UserDto>() // Inherit mappings from User to UserDto
            .ForMember(dest => dest.Warehouse, opt => opt.MapFrom(src => src.Warehouse)) // Map Warehouse to WarehouseDto
            .ForMember(dest => dest.WarehouseId, opt => opt.MapFrom(src => src.WarehouseId))
            .ForMember(dest => dest.isActive, opt => opt.MapFrom(src => src.isActive))
            .ForMember(dest => dest.AdministrativeRole, opt => opt.MapFrom(src => src.AdministrativeRole));

        CreateMap<BranchManagerDto, BranchManager>()
            .IncludeBase<UserDto, User>() // Inherit mappings from UserDto to User
            .ForMember(dest => dest.Warehouse, opt => opt.MapFrom(src => src.Warehouse))
            .ForMember(dest => dest.WarehouseId, opt => opt.MapFrom(src => src.WarehouseId))
            .ForMember(dest => dest.isActive, opt => opt.MapFrom(src => src.isActive))
            .ForMember(dest => dest.AdministrativeRole, opt => opt.MapFrom(src => src.AdministrativeRole));

        
        
        // PendingCustomer Mappings
        CreateMap<PendingCustomerForm, PendingCustomer>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
    }
}