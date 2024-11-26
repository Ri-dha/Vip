using System.Globalization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using VipProjectV0._1.Db;
using VipTest.attachmentsConfig;
using VipTest.Auth;
using VipTest.Files.Serivce;
using VipTest.Localization;
using VipTest.Rentals.Dto;
using VipTest.Rides.Dto;
using VipTest.Rides.Utli;
using VipTest.Users.Admins;
using VipTest.Users.customers;
using VipTest.Users.Drivers;
using VipTest.Users.Drivers.Dto;
using VipTest.Users.Drivers.Models;
using VipTest.Users.Drivers.PayLoads;
using VipTest.Users.Dtos;
using VipTest.Users.Models;
using VipTest.Users.OTP;
using VipTest.Users.PayLoad;
using VipTest.Utlity;
using VipTest.Wallets.PayLoads;
using VipTest.Wallets.Services;

namespace VipTest.Users.services;

public class UserServices : IUserServices
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly ILocalizationService _localize;
    private readonly IDtoTranslationService _dtoTranslationService;
    private readonly IWalletService _walletService;


    public UserServices(ITokenService tokenService, IMapper mapper, IRepositoryWrapper repositoryWrapper,
        ILocalizationService localize, IDtoTranslationService dtoTranslationService, IWalletService walletService)
    {
        _tokenService = tokenService;
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _localize = localize;
        _dtoTranslationService = dtoTranslationService;
        _walletService = walletService;
    }


    public async Task<(UserDto? user, string? error)> Delete(Guid id)
    {
        var user = await _repositoryWrapper.UserRepository.Get(u => u.Id == id);
        if (user == null)
        {
            var error = _localize.GetLocalizedString("UserNotFound");
            return (null, error);
        }

        ;
        user.Deleted = true;
        user = await _repositoryWrapper.UserRepository.Update(user, id);
        var userDto = _mapper.Map<UserDto>(user);
        return (userDto, null);
    }

    public async Task<(UserDto? user, string? error)> Get(Guid id)
    {
        var user = await _repositoryWrapper.UserRepository.Get(u => u.Id == id);
        if (user == null)
        {
            var error = _localize.GetLocalizedString("UserNotFound");
            return (null, error);
        }

        if (user.Role == Roles.Driver)
        {
            var driver = await _repositoryWrapper.DriverRepository.Get(d => d.Id == id,
                include: query => query.Include(v => v.Attachments)
                    .Include(v => v.Rides));
            var driverDto = _mapper.Map<DriverDto>(driver);
            driverDto = _dtoTranslationService.TranslateEnums(driverDto);
            return (driverDto, null);
        }

        if (user.Role == Roles.Customer)
        {
            var customer = await _repositoryWrapper.CustomerRepository.Get(c => c.Id == id
                , include: query => query.Include(v => v.FavoritePlaces)
                    .Include(v => v.Rides));
            var customerDto = _mapper.Map<CustomerDto>(customer);
            customerDto = _dtoTranslationService.TranslateEnums(customerDto);
            return (customerDto, null);
        }

        if (user.Role == Roles.Admin)
        {
            var admin = await _repositoryWrapper.AdminRepository.Get(a => a.Id == id);
            var adminDto = _mapper.Map<AdminDto>(admin);
            adminDto = _dtoTranslationService.TranslateEnums(adminDto);
            return (adminDto, null);
        }

        var userDto = _mapper.Map<UserDto>(user);
        return (userDto, null);
    }


    public async Task<(UserDto? user, string? error)> Login(LogInForm loginForm)
    {
        var user = await _repositoryWrapper.UserRepository.Get(u => u.PhoneNumber == loginForm.PhoneNumber);

        if (user == null)
        {
            var localizedError = _localize.GetLocalizedString("InvalidPassword");
            return (null, localizedError);
        }
        
        if (user.Deleted)
        {
            var localizedError = _localize.GetLocalizedString("UserDeleted");
            return (null, localizedError);
        }
        
        if(user.Role== Roles.Customer)
        {
            
            var customer = await _repositoryWrapper.CustomerRepository.Get(c => c.Id == user.Id);
            
            if (customer.CustomerStatus == CustomerStatus.Suspended)
            {
                var localizedError = _localize.GetLocalizedString("UserNotAllowed");
                return (null, localizedError);
            }
        }
        
        if(user.Role== Roles.Driver)
        {
            
            var driver = await _repositoryWrapper.DriverRepository.Get(c => c.Id == user.Id);
            
            if (driver.DriverStatus == DriverStatus.Suspended)
            {
                var localizedError = _localize.GetLocalizedString("UserNotAllowed");
                return (null, localizedError);
            }
        }
        
        if(user.Role== Roles.BranchManager)
        {
            var branchManager = await _repositoryWrapper.BranchManagerRepository.Get(c => c.Id == user.Id);   
            if (branchManager.isActive == false)
            {
                var localizedError = _localize.GetLocalizedString("UserNotAllowed");
                return (null, localizedError);
            }
        }


        if (!BCrypt.Net.BCrypt.Verify(loginForm.Password, user.Password))
        {
            var localizedError = _localize.GetLocalizedString("InvalidPassword");
            return (null, localizedError);
        }

        user.LastLogin = DateTime.Now;
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Token = _tokenService.CreateToken(user);
        userDto = _dtoTranslationService.TranslateEnums(userDto);
        return (userDto, null);
    }

    public async Task<(List<UserDto>? user, int? totalCount, string? error)> GetAll(UserFilter filter)
    {
        var (users, totalCount) = await _repositoryWrapper.UserRepository.GetAll<UserDto>(
            x => (
                (filter.UserName == null || x.Username.Contains(filter.UserName)) && // Applies if UserName is provided
                (filter.Email == null || x.Email.Contains(filter.Email)) && // Applies if Email is provided
                (filter.PhoneNumber == null ||
                 x.PhoneNumber.Contains(filter.PhoneNumber)) && // Applies if PhoneNumber is provided
                (filter.Role == null || x.Role == filter.Role) && // Applies if Role is provided
                // add a filter for deleted users
                (x.Deleted == false)
            ), filter.PageNumber, filter.PageSize);

        users.ForEach(x => x = _dtoTranslationService.TranslateEnums(x));
        return (users, totalCount, null);
    }

    public async Task<(List<UserDto>? user, int? totalCount, string? error)> GetAllCustomers(CustomerFilter filter)
    {
        var (customers, totalCount) = await _repositoryWrapper.CustomerRepository.GetAll<UserDto>(
            x => (
                (filter.UserName == null || x.Username.Contains(filter.UserName)) && // Applies if UserName is provided
                (filter.Email == null || x.Email.Contains(filter.Email)) && // Applies if Email is provided
                (filter.PhoneNumber == null ||
                 x.PhoneNumber.Contains(filter.PhoneNumber)) && // Applies if PhoneNumber is provided
                (filter.Role == null || x.Role == filter.Role) && // Applies if Role is provided
                (filter.CustomerStatus == null || x.CustomerStatus == filter.CustomerStatus) && // Applies if CustomerStatus is provided
                // add a filter for deleted users
                (x.Deleted == false)
            ), filter.PageNumber, filter.PageSize);

        customers.ForEach(x => x = _dtoTranslationService.TranslateEnums(x));
        return (customers, totalCount, null);
    }

    public async Task<(List<UserDto>? user, int? totalCount, string? error)> GetAllAdmins(AdminFilter filter)
    {
        
        var (admins, totalCount) = await _repositoryWrapper.AdminRepository.GetAll<UserDto>(
            x => (
                (filter.UserName == null || x.Username.Contains(filter.UserName)) && // Applies if UserName is provided
                (filter.Email == null || x.Email.Contains(filter.Email)) && // Applies if Email is provided
                (filter.PhoneNumber == null ||
                 x.PhoneNumber.Contains(filter.PhoneNumber)) && // Applies if PhoneNumber is provided
                (filter.Role == null || x.Role == filter.Role) && // Applies if Role is provided
                (filter.AdministrativeRole == null || x.AdministrativeRole == filter.AdministrativeRole) && // Applies if AdministrativeRole is provided
                // add a filter for deleted users
                (x.Deleted == false)
            ), filter.PageNumber, filter.PageSize);

        admins.ForEach(x => x = _dtoTranslationService.TranslateEnums(x));
        return (admins, totalCount, null);
        
    }

    public async Task<(List<UserDto>? user, int? totalCount, string? error)> GetAllDrivers(DriverFilter filter)
    {
        var (drivers, totalCount) = await _repositoryWrapper.DriverRepository.GetAll<UserDto>(
            x => (
                (filter.UserName == null || x.Username.Contains(filter.UserName)) && // Applies if UserName is provided
                (filter.Email == null || x.Email.Contains(filter.Email)) && // Applies if Email is provided
                (filter.PhoneNumber == null ||
                 x.PhoneNumber.Contains(filter.PhoneNumber)) && // Applies if PhoneNumber is provided
                (filter.Role == null || x.Role == filter.Role) && // Applies if Role is provided
                (filter.DriverStatus == null || x.DriverStatus == filter.DriverStatus) && // Applies if DriverStatus is provided
                // add a filter for deleted users
                (x.Deleted == false)
            ), filter.PageNumber, filter.PageSize);

        drivers.ForEach(x => x = _dtoTranslationService.TranslateEnums(x));
        return (drivers, totalCount, null);
    }

    public async Task<(List<object>? rolesList, string? error)> GetRoles()
    {
        var roles = Enum.GetValues<Roles>()
            .Cast<Roles>()
            .Select(role => new { Name = role.ToString(), Value = (int)role })
            .ToList<object>();
        return (roles, null);
    }

    public async Task<(List<object>? adminRolesList, string? error)> GetAdminRoles()
    {
        var roles = Enum.GetValues<AdministrativeRoles>()
            .Cast<AdministrativeRoles>()
            .Select(role => new { Name = role.ToString(), Value = (int)role })
            .ToList<object>();

        return (roles, null);
    }


    public async Task<(UserDto? userDto, string? error)> RegisterDriver(DriverForm driverForm)
    {
        var user = await _repositoryWrapper.UserRepository.Get(u => u.PhoneNumber == driverForm.PhoneNumber);
        if (user != null)
        {
            var localizedError = _localize.GetLocalizedString("UserAlreadyExists");
            return (null, localizedError);
        }

        var driver = new Driver
        {
            Username = driverForm.Username,
            Email = driverForm.Email,
            PhoneNumber = driverForm.PhoneNumber,
            Password = BCrypt.Net.BCrypt.HashPassword(driverForm.Password),
            Role = Roles.Driver,
            ProfileImage = driverForm.ProfileImage,
            LicenseFile = driverForm.DriverLicense,
            IdFile = driverForm.DriverIdFile,
            Attachments = _mapper.Map<List<Attachments>>(driverForm.Attachments) // Map attachments
        };

        _mapper.Map(driverForm, driver);

        driver = await _repositoryWrapper.DriverRepository.Add(driver);
        var driverDto = _mapper.Map<DriverDto>(driver);

        var driverGroup = await _repositoryWrapper.UserGroupsRepository.Get(x => x.code == "2");
        driverGroup.userIds.Add(driver.Id);
        await _repositoryWrapper.UserGroupsRepository.Update(driverGroup, driverGroup.Id);
        
        await _walletService.CreateWalletAsync(new CreateWalletForm()
        {
            Name = driver.Username,
            UserId = driver.Id
        });

        return (driverDto, null);
    }

    public async Task<(UserDto? userDto, string? error)> UpdateDriver(DriverUpdateForm driverUpdateForm, Guid id)
    {
        var driver = await _repositoryWrapper.DriverRepository.Get(d => d.Id == id,
            include: q=>q.Include(x=>x.Attachments)
            );
        if (driver == null)
        {
            var error = _localize.GetLocalizedString("UserNotFound");
            return (null, error);
        }

        if (driver.Role != Roles.Driver)
        {
            var error = _localize.GetLocalizedString("UserIsNotADriver");
            return (null, error);
        }

        _mapper.Map(driverUpdateForm, driver);

        // Handle attachments if provided
        if (driverUpdateForm.Attachments != null)
        {
            driver.Attachments.RemoveAll(x => x.Id != null);
            driver.Attachments = _mapper.Map<List<Attachments>>(driverUpdateForm.Attachments);
        }

        driver = await _repositoryWrapper.DriverRepository.Update(driver, id);
        var driverDto = _mapper.Map<DriverDto>(driver);
        return (driverDto, null);
    }

    public async Task<(UserDto? userDto, string? error)> RegisterCustomer(CustomerForm customerForm)
    {
        var user = await _repositoryWrapper.UserRepository.Get(u => u.PhoneNumber == customerForm.PhoneNumber);
        if (user != null)
        {
            var localizedError = _localize.GetLocalizedString("UserAlreadyExists");
            return (null, localizedError);
        }

        // Handle file uploads manually

        // Explicitly create a new instance of Customer and set the properties
        var customer = new Customer
        {
            Username = customerForm.Username,
            Email = customerForm.Email,
            PhoneNumber = customerForm.PhoneNumber,
            Password = BCrypt.Net.BCrypt.HashPassword(customerForm.Password), // Hash the password
            Role = Roles.Customer,
            ProfileImage = customerForm.ProfileImage,
            LicenseFile = customerForm.CustomerLicenseFile,
            IdFile = customerForm.CustomerIdFile
        };
        // Use AutoMapper to map the rest of the properties (ignoring password and file properties)
        _mapper.Map(customerForm, customer);

        // Save the customer to the database using the CustomerRepository
        customer = await _repositoryWrapper.CustomerRepository.Add(customer);

        // Map the saved customer to a CustomerDto to return
        var customerDto = _mapper.Map<CustomerDto>(customer);
        customerDto.Token = _tokenService.CreateToken(customer);
        var customerGroup = await _repositoryWrapper.UserGroupsRepository.Get(x => x.code == "1");
        customerGroup.userIds.Add(customer.Id);
        await _repositoryWrapper.UserGroupsRepository.Update(customerGroup, customerGroup.Id);
        
        return (customerDto, null);
    }

    public async Task<(UserDto? userDto, string? error)> UpdateCustomer(CustomerUpdateForm customerUpdateForm, Guid id)
    {
        // Retrieve the existing customer from the database
        var customer = await _repositoryWrapper.CustomerRepository.Get(c => c.Id == id);

        if (customer == null)
        {
            var error = _localize.GetLocalizedString("UserNotFound");
            return (null, error);
        }

        if (customer.Role != Roles.Customer)
        {
            var error = _localize.GetLocalizedString("UserIsNotACustomer");
            return (null, error);
        }

        // Use AutoMapper to update the properties, excluding the password
        _mapper.Map(customerUpdateForm, customer);

        // Check if the password is being updated, and hash it if it is
        if (!string.IsNullOrEmpty(customerUpdateForm.Password))
        {
            customer.Password = BCrypt.Net.BCrypt.HashPassword(customerUpdateForm.Password);
        }

        // Save updated customer to the database
        customer = await _repositoryWrapper.CustomerRepository.Update(customer, id);

        // Map the updated customer to CustomerDto
        var customerDto = _mapper.Map<CustomerDto>(customer);
        return (customerDto, null);
    }

    public async Task<(UserDto? userDto, string? error)> ChangePassword(string oldPassword, string newPassword, Guid id)
    {
        var user = await _repositoryWrapper.UserRepository.Get(u => u.Id == id);
        if (user == null)
        {
            var error = _localize.GetLocalizedString("UserNotFound");
            return (null, error);
        }

        if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.Password))
        {
            var error = _localize.GetLocalizedString("InvalidPassword");
            return (null, error);
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user = await _repositoryWrapper.UserRepository.Update(user, id);
        var userDto = _mapper.Map<UserDto>(user);
        return (userDto, null);
    }

    public async Task<(bool, string? error)> VerifyOtp(string phoneNumber, string otp)
    {
        var customer = await _repositoryWrapper.UserRepository.Get(c => c.PhoneNumber == phoneNumber);
        if (customer == null)
        {
            var localizedError = _localize.GetLocalizedString("UserNotFound");
            return (false, localizedError);
        }
        
        if (otp != "12345")
        {
            var localizedError = _localize.GetLocalizedString("InvalidOTP");
            return (false, localizedError);
        }
        
        return (true, null);
    }

    public async Task<(string? message, string? error)> ForgetPassword(ForgetPasswordForm forgetPasswordForm)
    {
        // Find user by phone number
        var user = await _repositoryWrapper.UserRepository.Get(u => u.PhoneNumber == forgetPasswordForm.PhoneNumber);

        if (user == null)
        {
            var localizedError = _localize.GetLocalizedString("UserNotFound");
            return (null, localizedError);
        }


        // Save the user with updated password
        await _repositoryWrapper.UserRepository.Update(user, user.Id);

        var localizedMessage = _localize.GetLocalizedString("PasswordResetSuccessfully");
        return (localizedMessage, null);
    }

    public async Task<(UserDto? userDto, string? error)> ChangePassword(ChangePasswordForm changePasswordForm)
    {
        
        var user = await _repositoryWrapper.UserRepository.Get(u => u.PhoneNumber == changePasswordForm.PhoneNumber);
        if (user == null)
        {
            var error = _localize.GetLocalizedString("UserNotFound");
            return (null, error);
        }
        user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordForm.NewPassword);
        user = await _repositoryWrapper.UserRepository.Update(user, user.Id);
        var userDto = _mapper.Map<UserDto>(user);
        return (userDto, null);
        
    }

    public async Task<(UserDto? userDto, string? error)> RegisterAdmin(AdminForm adminForm)
    {
        var user = await _repositoryWrapper.UserRepository.Get(u => u.PhoneNumber == adminForm.PhoneNumber);
        if (user != null)
        {
            var localizedError = _localize.GetLocalizedString("UserAlreadyExists");
            return (null, localizedError);
        }

        var admin = new Admin
        {
            Username = adminForm.Username,
            Email = adminForm.Email,
            PhoneNumber = adminForm.PhoneNumber,
            Password = BCrypt.Net.BCrypt.HashPassword(adminForm.Password),
            Role = Roles.Admin,
            ProfileImage = adminForm.ProfileImage
        };

        _mapper.Map(adminForm, admin);
        admin = await _repositoryWrapper.AdminRepository.Add(admin);
        var adminDto = _mapper.Map<AdminDto>(admin);

        var adminGroup = await _repositoryWrapper.UserGroupsRepository.Get(x => x.code == "0");
        adminGroup.userIds.Add(admin.Id);
        await _repositoryWrapper.UserGroupsRepository.Update(adminGroup, adminGroup.Id);
        return (adminDto, null);
    }

    public async Task<(UserDto? userDto, string? error)> UpdateAdmin(AdminUpdateForm updateForm, Guid id)
    {
        var admin = await _repositoryWrapper.AdminRepository.Get(u => u.Id == id);
        if (admin == null)
        {
            var error = _localize.GetLocalizedString("UserNotFound");
            return (null, error);
        }

        if (admin.Role != Roles.Admin)
        {
            var error = _localize.GetLocalizedString("UserIsNotADriver");
            return (null, error);
        }

        _mapper.Map(updateForm, admin);
        if (!string.IsNullOrEmpty(updateForm.Password))
        {
            admin.Password = BCrypt.Net.BCrypt.HashPassword(updateForm.Password);
        }

        admin = await _repositoryWrapper.AdminRepository.Update(admin, id);
        var adminDto = _mapper.Map<AdminDto>(admin);
        return (adminDto, null);
    }

    public async Task<(string? userDto, string? error)> CreatePendingCustomer(PendingCustomerForm pendingCustomerForm)
    {
        var user = await _repositoryWrapper.UserRepository.Get(u => u.PhoneNumber == pendingCustomerForm.PhoneNumber);
        if (user != null)
        {
            var localizedError = _localize.GetLocalizedString("UserAlreadyExists");
            return (null, localizedError);
        }

        var pendingCustomer = new PendingCustomer(
            username: pendingCustomerForm.Username,
            phoneNumber: pendingCustomerForm.PhoneNumber,
            password: BCrypt.Net.BCrypt.HashPassword(pendingCustomerForm.Password)
        );

        await _repositoryWrapper.PendingCustomerRepository.Add(pendingCustomer);
        return (null, null);
    }

    public async Task<(UserDto? userDto, string? error)> VerifyPendingCustomer(string phoneNumber, string otp)
    {
        if (otp == "12345")
        {
            var pendingCustomer =
                await _repositoryWrapper.PendingCustomerRepository.GetLatestPendingCustomerByPhoneNumber(phoneNumber);
            if (pendingCustomer == null)
            {
                var localizedError = _localize.GetLocalizedString("UserNotFound");
                return (null, localizedError);
            }

            var customer = new Customer
            {
                Username = pendingCustomer.Username,
                PhoneNumber = pendingCustomer.PhoneNumber,
                Password = pendingCustomer.Password,
                Role = Roles.Customer,
                CustomerStatus = CustomerStatus.Active
                
            };

            var ListOfPendingCustomers =
                await _repositoryWrapper.PendingCustomerRepository.GetAllPendingCustomerByPhoneNumber(phoneNumber);
            var ListOfPendingCustomersId = ListOfPendingCustomers.Select(x => x.Id).ToList();
            await _repositoryWrapper.PendingCustomerRepository.RemoveAll(ListOfPendingCustomersId);
            await _repositoryWrapper.CustomerRepository.Add(customer);
            var customerDto = _mapper.Map<CustomerDto>(customer);
            customerDto.Token = _tokenService.CreateToken(customer);
            
            await _walletService.CreateWalletAsync(new CreateWalletForm()
            {
                Name = customer.Username,
                UserId = customer.Id
            });

            return (customerDto, null);
        }
        else
        {
            var localizedError = _localize.GetLocalizedString("InvalidOTP");
            return (null, localizedError);
        }
    }

    public async Task<(DriverInfoDto? userDto, string? error)> GetDriverInfo(Guid driverId, int pageNumber = 1,
        int pageSize = 30)
    {
        var todayStart = DateTime.UtcNow.Date; // Use UTC date
        var todayEnd = todayStart.AddDays(1).AddTicks(-1); // End of the current UTC day

        var driver = await _repositoryWrapper.DriverRepository.Get(d => d.Id == driverId,
            include: query => query.Include(v => v.Attachments)
                .Include(v => v.Rides)
                .Include(v => v.CarRentalOrders));

        if (driver == null)
        {
            var error = _localize.GetLocalizedString("UserNotFound");
            return (null, error);
        }

        var dto = new DriverInfoDto()
        {
            TotalRides = driver.TripCount
        };

        var totalRides = await _repositoryWrapper.RideRepository.Count(x => x.DriverId == driverId);
        var completedRides =
            await _repositoryWrapper.RideRepository.Count(x =>
                x.DriverId == driverId && x.Status == RideStatus.Completed);
        var canceledRides =
            await _repositoryWrapper.RideRepository.Count(
                x => x.DriverId == driverId && x.Status == RideStatus.Canceled);


        // Protect against division by zero
        if (totalRides > 0)
        {
            var completedRidesPercentage = (int)Math.Round((decimal)((double)completedRides / totalRides * 100));
            var canceledRidesPercentage = (int)Math.Round((decimal)((double)canceledRides / totalRides * 100));
            dto.CompletedRidesPercentage = completedRidesPercentage;
            dto.CanceledRidesPercentage = canceledRidesPercentage;
        }
        else
        {
            dto.CompletedRidesPercentage = 0;
            dto.CanceledRidesPercentage = 0;
        }

        dto.Rating = driver.Rating;

        var theFirstRideForToday = await _repositoryWrapper.RideRepository.Get(x =>
            x.DriverId == driverId && x.CreatedAt >= todayStart && x.CreatedAt <= todayEnd);
        var theLastCarRentalOrder = await _repositoryWrapper.CarRentalOrderRepository.Get(x =>
            x.DriverId == driverId && x.CreatedAt >= todayStart && x.CreatedAt <= todayEnd);


        // Fetch today's rides and total count
        var (ridesForToday, totalRidesForToday) = await _repositoryWrapper.RideRepository.GetAll<RideDto>(
            x => x.DriverId == driverId && x.CreatedAt >= todayStart && x.CreatedAt <= todayEnd,
            pageNumber, pageSize);

        var (carRentalOrdersForToday, totalCarRentalOrdersForToday) =
            await _repositoryWrapper.CarRentalOrderRepository.GetAll<CarRentalOrderDto>(
                x => x.DriverId == driverId && x.CreatedAt >= todayStart && x.CreatedAt <= todayEnd,
                pageNumber, pageSize);

        // Map the latest ride and rides for today
        dto.LastestRide = _mapper.Map<RideDto>(theFirstRideForToday);
        dto.RidesForToday = ridesForToday; // No mapping needed as ridesForToday is already List<RideDto>

        dto.LastestCarRentalOrder = _mapper.Map<CarRentalOrderDto>(theLastCarRentalOrder);
        dto.CarRentalOrders =
            carRentalOrdersForToday; // No mapping needed as carRentalOrdersForToday is already List<CarRentalOrderDto>

        var ridesDto= dto.LastestRide;
        _dtoTranslationService.TranslateEnums(ridesDto);
        dto.LastestRide = ridesDto;
        
        // var carRentalOrderDto = dto.LastestCarRentalOrder;
        // _dtoTranslationService.TranslateEnums(carRentalOrderDto);
        // dto.LastestCarRentalOrder = carRentalOrderDto;
        
        var ridesForTodayDto = dto.RidesForToday;
        ridesForTodayDto.ForEach(x => x = _dtoTranslationService.TranslateEnums(x));
        dto.RidesForToday = ridesForTodayDto;
        
        return (dto, null);
    }

    public async Task<(string? lang, string? error)> UpdateLanguage(bool lang, Guid userId)
    {
        var customer = await _repositoryWrapper.CustomerRepository.Get(c => c.Id == userId);
        if (customer == null)
        {
            var error = _localize.GetLocalizedString("UserNotFound");
            return (null, error);
        }
        customer.IsEnglish = lang;
        await _repositoryWrapper.CustomerRepository.Update(customer, userId);
        return (lang ? "en" : "ar", null);
    }
}