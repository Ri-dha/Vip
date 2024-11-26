using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VipTest.Localization;
using VipTest.Users.Admins;
using VipTest.Users.customers;
using VipTest.Users.Drivers;
using VipTest.Users.Drivers.PayLoads;
using VipTest.Users.Dtos;
using VipTest.Users.Models;
using VipTest.Users.OTP;
using VipTest.Users.PayLoad;
using VipTest.Users.services;
using VipTest.Utlity;
using VipTest.Utlity.Basic;

namespace VipTest.Users.controller;

[Route("users/")]
public class UserController : BaseController
{
    private readonly IUserServices _userServices;
    private readonly IEnumTranslationService _enumTranslationService;

    public UserController(IUserServices userServices, IEnumTranslationService enumTranslationService)
    {
        _userServices = userServices;
        _enumTranslationService = enumTranslationService;
    }

    [HttpPost("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] AdminForm adminForm)
    {
        var (user, error) = await _userServices.RegisterAdmin(adminForm);
        if (error != null) return BadRequest(new { error });
        return Ok(user);
    }

    [HttpPut("update-admin/{id}")]
    public async Task<IActionResult> UpdateAdmin(Guid id, [FromBody] AdminUpdateForm updateForm)
    {
        var (adminDto, error) = await _userServices.UpdateAdmin(updateForm, id);
        if (error != null)
        {
            return BadRequest(new { error });
        }

        return Ok(adminDto);
    }

    [HttpPost("register-customer")]
    [AllowAnonymous]
    public async Task<IActionResult> CreatePendingCustomer([FromBody] PendingCustomerForm pendingCustomerForm)
    {
        var (user, error) = await _userServices.CreatePendingCustomer(pendingCustomerForm);
        if (error != null) return BadRequest(new { error });
        return Ok(user);
    }

    [HttpPost("verify-customer")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyPendingCustomer(String phoneNumber, String otp)
    {
        var (user, error) = await _userServices.VerifyPendingCustomer(phoneNumber, otp);
        if (error != null) return BadRequest(new { error });
        return Ok(user);
    }

    [HttpPut("update-customer/{id}")]
    public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] CustomerUpdateForm customerUpdateForm)
    {
        var (customerDto, error) = await _userServices.UpdateCustomer(customerUpdateForm, id);
        if (error != null)
        {
            return BadRequest(new { error });
        }

        return Ok(customerDto);
    }

    [HttpGet("get-driver-info/{id}")]
    public async Task<IActionResult> GetDriverInfo(Guid id)
    {
        var (driver, error) = await _userServices.GetDriverInfo(id);
        if (error != null) return BadRequest(new { error });
        return Ok(driver);
    }

    [HttpPost("register-driver")]
    public async Task<IActionResult> RegisterDriver([FromBody] DriverForm driverForm)
    {
        var (user, error) = await _userServices.RegisterDriver(driverForm);
        if (error != null) return BadRequest(new { error });
        return Ok(user);
    }

    [HttpPut("update-driver/{id}")]
    public async Task<IActionResult> UpdateDriver(Guid id, [FromBody] DriverUpdateForm driverUpdateForm)
    {
        var (driverDto, error) = await _userServices.UpdateDriver(driverUpdateForm, id);
        if (error != null)
        {
            return BadRequest(new { error });
        }

        return Ok(driverDto);
    }


    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var (user, error) = await _userServices.Delete(id);
        if (error != null) return BadRequest(new { error });
        return Ok(user);
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var (user, error) = await _userServices.Get(id);
        if (error != null) return BadRequest(new { error });
        return Ok(user);
    }

    [HttpGet("get-all")]
    public async Task<ActionResult<Page<UserDto>>> GetAll([FromQuery] UserFilter filter)
    {
        var (users, totalCount, error) = await _userServices.GetAll(filter);
        if (error != null) return BadRequest(new { error });
        return Ok(new { data = users, totalCount, filter.PageNumber, filter.PageSize });
    }

    [HttpGet("get-all-customers")]
    public async Task<ActionResult<Page<UserDto>>> GetAllCsuomers([FromQuery] CustomerFilter filter)
    {
        var (users, totalCount, error) = await _userServices.GetAllCustomers(filter);
        if (error != null) return BadRequest(new { error });
        return Ok(new { data = users, totalCount, filter.PageNumber, filter.PageSize });
    }

    [HttpGet("get-all-admins")]
    public async Task<ActionResult<Page<UserDto>>> GetAllAdmins([FromQuery] AdminFilter filter)
    {
        var (users, totalCount, error) = await _userServices.GetAllAdmins(filter);
        if (error != null) return BadRequest(new { error });
        return Ok(new { data = users, totalCount, filter.PageNumber, filter.PageSize });
    }
    
    [HttpGet("get-all-drivers")]
    public async Task<ActionResult<Page<UserDto>>> GetAllDrivers([FromQuery] DriverFilter filter)
    {
        var (users, totalCount, error) = await _userServices.GetAllDrivers(filter);
        if (error != null) return BadRequest(new { error });
        return Ok(new { data = users, totalCount, filter.PageNumber, filter.PageSize });
    }


    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LogInForm loginForm)
    {
        var (user, error) = await _userServices.Login(loginForm);
        if (error != null) return BadRequest(new { error });
        return Ok(user);
    }


    [HttpPost("forget-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordForm forgetPasswordForm)
    {
        var (message, error) = await _userServices.ForgetPassword(forgetPasswordForm);
        if (error != null) return BadRequest(new { error });
        return Ok(message);
    }

    [HttpPost("verify-otp")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyOtp(string phoneNumber, string otp)
    {
        var (user, error) = await _userServices.VerifyOtp(phoneNumber, otp);
        if (error != null) return BadRequest(new { error });
        return Ok(user);
    }

    [HttpPut("change-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordForm changePasswordForm)
    {
        var (user, error) = await _userServices.ChangePassword(changePasswordForm);
        if (error != null) return BadRequest(new { error });
        return Ok(user);
    }


    [HttpPut("change-password/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> ChangePassword(Guid id, string oldPassword, string newPassword)
    {
        var (user, error) = await _userServices.ChangePassword(oldPassword, newPassword, id);
        if (error != null) return BadRequest(new { error });
        return Ok(user);
    }

    [HttpGet("get-driver-status")]
    [AllowAnonymous]
    public IActionResult GetDriverStatusTranslation()
    {
        var driverStatuses = _enumTranslationService.GetAllEnumTranslations<DriverStatus>();
        return Ok(driverStatuses);
    }

    [HttpGet("get-admin-roles")]
    [AllowAnonymous]
    public IActionResult GetAdminRolesTranslation()
    {
        var roles = _enumTranslationService.GetAllEnumTranslations<AdministrativeRoles>();
        var response = new { data = roles };
        return Ok(response);
    }


    [HttpGet("get-roles")]
    [AllowAnonymous]
    public IActionResult GetRolesTranslation()
    {
        var roles = _enumTranslationService.GetAllEnumTranslations<Roles>();
        return Ok(roles);
    }

    [HttpGet("get-customer-statuses")]
    [AllowAnonymous]
    public IActionResult GetCustomerStatusTranslations()
    {
        var customerStatuses = _enumTranslationService.GetAllEnumTranslations<CustomerStatus>();
        return Ok(customerStatuses);
    }

    // [HttpPut("update-language/{userId}")]
    // public async Task<IActionResult> UpdateLanguage(bool lang, Guid userId)
    // {
    //     var (language, error) = await _userServices.UpdateLanguage(lang, userId);
    //     if (error != null) return BadRequest(new { error });
    //     return Ok(language);
    // }
}