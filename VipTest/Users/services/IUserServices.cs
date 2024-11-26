using VipTest.Users.Admins;
using VipTest.Users.customers;
using VipTest.Users.Drivers.Dto;
using VipTest.Users.Drivers.PayLoads;
using VipTest.Users.Dtos;
using VipTest.Users.Models;
using VipTest.Users.OTP;
using VipTest.Users.PayLoad;

namespace VipTest.Users.services;

public interface IUserServices
{
    Task<(UserDto? user, string? error)> Delete(Guid id);
    Task<(UserDto? user, string? error)> Get(Guid id);
    Task<(UserDto? user, string? error)> Login(LogInForm loginForm);
    
    Task<(List<UserDto>? user, int? totalCount, string? error)> GetAll(UserFilter filter);
    Task<(List<UserDto>? user, int? totalCount, string? error)> GetAllCustomers(CustomerFilter filter);
    Task<(List<UserDto>? user, int? totalCount, string? error)> GetAllAdmins(AdminFilter filter);
    Task<(List<UserDto>? user, int? totalCount, string? error)> GetAllDrivers(DriverFilter filter);
    Task<(List<object>? rolesList, string? error)> GetRoles();
    Task<(List<object>? adminRolesList, string? error)> GetAdminRoles();
    
    
    Task<(UserDto? userDto, string? error)> RegisterDriver(DriverForm driverForm);
    Task<(UserDto? userDto, string? error)> UpdateDriver(DriverUpdateForm driverUpdateForm, Guid id);
   
    Task<(UserDto? userDto, string? error)> RegisterCustomer(CustomerForm customerForm);
    Task<(UserDto? userDto, string? error)> UpdateCustomer(CustomerUpdateForm customerUpdateForm, Guid id);
    Task<(UserDto? userDto, string? error)> ChangePassword(string oldPassword,string newPassword, Guid id);
    Task<(bool, string? error)> VerifyOtp(string phoneNumber, string otp);
    Task<(string? message, string? error)> ForgetPassword(ForgetPasswordForm forgetPasswordForm);
    Task<(UserDto? userDto, string? error)> ChangePassword(ChangePasswordForm changePasswordForm);
    
    Task<(UserDto? userDto, string? error)> RegisterAdmin(AdminForm adminForm);
    Task<(UserDto? userDto, string? error)> UpdateAdmin(AdminUpdateForm updateForm, Guid id);
    
    
    Task<(string? userDto, string? error)> CreatePendingCustomer(PendingCustomerForm pendingCustomerForm);
    
    Task<(UserDto? userDto, string? error)> VerifyPendingCustomer(String phoneNumber, String otp);
    Task<(DriverInfoDto? userDto, string? error)> GetDriverInfo(Guid driverId, int pageNumber = 1, int pageSize = 20);
    
    
    Task<(string? lang,string?error)> UpdateLanguage(bool lang, Guid userId); 
    
}