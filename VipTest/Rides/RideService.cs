using System.Net.Mail;
using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VipProjectV0._1.Db;
using VipTest.attachmentsConfig;
using VipTest.Localization;
using VipTest.Notifications;
using VipTest.Notifications.Models;
using VipTest.Notifications.PayLoads;
using VipTest.RideBillings;
using VipTest.Rides.Dto;
using VipTest.Rides.Models;
using VipTest.Rides.Payloads;
using VipTest.Rides.Utli;
using VipTest.Transactions.models;
using VipTest.Transactions.utli;
using VipTest.Users.customers;
using VipTest.Users.Drivers;
using VipTest.Users.Models;
using VipTest.vehicles.Utli;

namespace VipTest.Rides;

public interface IRideService
{
    Task<(RideDto? rideDto, string? error)> CreateRideAsync(RideCreateForm rideCreateForm);
    Task<(RideDto? rideDto, string? error)> UpdateRideAsync(Guid id, RideUpdateForm rideUpdateForm);
    Task<(RideDto? rideDto, string? error)> GetRideByIdAsync(Guid id);
    Task<(List<RideDto>? rideDtos, int? totalCount, string? error)> GetAllRidesAsync(RideFilterForm filterForm);
    Task<(RideDto? rideDto, string? error)> DeleteRideAsync(Guid id);
    Task<(RideDto? rideDto, string? error)> ConfirmRideAsync(Guid id);
    Task<(RideDto? rideDto, string? error)> RejectRideAsync(Guid id, string? rejectionReason);
    Task<(RideDto? rideDto, string? error)> CancelRideAsync(Guid id, string? cancelationReason);
    Task<(RideDto? rideDto, string? error)> StartRideAsync(Guid id);
    Task<(RideDto? rideDto, string? error)> CompleteRideAsync(Guid id);
    Task<(RideDto? rideDto, string? error)> ArrivedAtPickupAsync(Guid id);
    Task<(RideDto? rideDto, string? error)> ArrivedAtDetourAsync(Guid id);


    Task<(RideDto? rideDto, string? error)> ChangeRideStatus(Guid id, RideStatus status, string? note);
    Task<(int CompletedRidesPercentage, int CanceledRidesPercentage)> GetDriverRideStatistics(Guid driverId);
}

public class RideService : IRideService
{
    private readonly IRepositoryWrapper _repo;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localize;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserNotificationServices _notification;
    private readonly IDtoTranslationService _dtoTranslationService;


    public RideService(ILocalizationService localize, IMapper mapper, IRepositoryWrapper repo,
        IHttpContextAccessor httpContextAccessor, IUserNotificationServices notification,
        IDtoTranslationService dtoTranslationService)
    {
        _localize = localize;
        _mapper = mapper;
        _repo = repo;
        _httpContextAccessor = httpContextAccessor;
        _notification = notification;
        _dtoTranslationService = dtoTranslationService;
    }


    public async Task<(RideDto? rideDto, string? error)> CreateRideAsync(RideCreateForm rideCreateForm)
    {
        // Fetch the ride billing configuration for the given ride type
        var rideBillingConfig = await _repo.RideBillingRepository.Get(x => x.RideType == rideCreateForm.RideType);
        if (rideBillingConfig == null)
        {
            return (null, _localize.GetLocalizedString("FailedToGetRideBilling"));
        }

        var settings = await _repo.SettingsRepository.Get(x => true);
        if (settings == null)
        {
            return (null, _localize.GetLocalizedString("SettingsNotFound"));
        }

        if (rideCreateForm.PickupTime < DateTime.Now)
        {
            return (null, _localize.GetLocalizedString("PickupTimeInThePast"));
        }


        var ride = _mapper.Map<Ride>(rideCreateForm);

        if (ride.RideType == RideType.Vip)
        {
            rideCreateForm.VipPassportPackage = true;
            rideCreateForm.MissingBaggagePackage = true;
            rideCreateForm.WelcomePackage = true;
            ride.VipPassportPackage = true;
            ride.MissingBaggagePackage = true;
            ride.WelcomePackage = true;
        }


        ride.RidingCode = await GenerateRideCodeAsync();

        // Calculate the base fare price
        ride.Price = rideBillingConfig.BaseFarePrice;

        // Add the detour fare price if there's a detour
        if (rideCreateForm.IsDetour.HasValue && rideCreateForm.IsDetour.Value)
        {
            ride.Price += rideBillingConfig.DetourFarePrice;
        }
        
        if (rideCreateForm.RideType == RideType.Normal|| rideCreateForm.RideType == RideType.Normal_To_Babil
            || rideCreateForm.RideType == RideType.Normal_To_DhiQar || rideCreateForm.RideType == RideType.Normal_To_Karbala
            || rideCreateForm.RideType == RideType.Normal_To_Maysan || rideCreateForm.RideType == RideType.Normal_To_Najaf
            || rideCreateForm.RideType == RideType.Normal_To_Qadisiyah || rideCreateForm.RideType == RideType.Normal_To_Wasit)
        {
            if (rideCreateForm.VipPassportPackage == true)
            {
                var passportsCommission = settings.VisaCommission;
                ride.VipPassportPackage = true;
                ride.VisaCommission = passportsCommission;
                ride.Price += passportsCommission;

                if (rideCreateForm.Attachments != null && rideCreateForm.Attachments.Count > 0)
                {
                    ride.Attachments = _mapper.Map<List<Attachments>>(rideCreateForm.Attachments);
                }
            }

            if (rideCreateForm.MissingBaggagePackage == true)
            {
                var missingBaggageCommission = settings.MissingBaggageCommission;
                ride.MissingBaggagePackage = true;
                ride.MissingBaggageCommission = missingBaggageCommission;
                ride.Price += missingBaggageCommission;

                if (rideCreateForm.Attachments != null && rideCreateForm.Attachments.Count > 0)
                {
                    ride.Attachments = _mapper.Map<List<Attachments>>(rideCreateForm.Attachments);
                }
            }

            if (rideCreateForm.WelcomePackage == true)
            {
                var welcomePackageCommission = settings.WelcomePackageCommission;
                ride.WelcomePackage = true;
                ride.WelcomePackageCommission = welcomePackageCommission;
                ride.Price += welcomePackageCommission;

                if (rideCreateForm.Attachments != null && rideCreateForm.Attachments.Count > 0)
                {
                    ride.Attachments = _mapper.Map<List<Attachments>>(rideCreateForm.Attachments);
                }
            }
        }

        // Check if a discount code is provided
        if (!string.IsNullOrEmpty(rideCreateForm.DiscountCode))
        {
            var discount = await _repo.DiscountRepository.Get(x => x.Code == rideCreateForm.DiscountCode);
            if (discount != null &&
                discount.IsValidForRide(rideCreateForm.CustomerId, DateTime.Now, rideCreateForm.RideType))
            {
                // Apply the discount and calculate the final price
                ride.ApplyDiscount(discount);
                ride.DiscountId = discount.Id;
                ride.Discount = discount;
            }
            else
            {
                // If the discount code is invalid or not applicable, set the final price to the original price
                ride.FinalPrice = ride.Price;
            }
        }
        else
        {
            // If no discount code is provided, set the final price to the original price
            ride.FinalPrice = ride.Price;
        }


        // Fetch the customer
        var customer = await _repo.CustomerRepository.Get(x => x.Id == rideCreateForm.CustomerId);
        if (customer == null)
        {
            return (null, _localize.GetLocalizedString("CustomerNotFound"));
        }

        if (customer.CustomerStatus == CustomerStatus.Suspended)
        {
            return (null, _localize.GetLocalizedString("UserNotAllowed"));
        }

        ride.CustomerId = customer.Id;
        ride.Customer = customer;

        // Add the ride to the customer, driver, and vehicle entities
        customer.Rides.Add(ride);
        // await _repo.CustomerRepository.Update(customer, customer.Id);
        ride.NumberOfPassengers = rideCreateForm.NumberOfPassengers;
        ride.CustomerNote = rideCreateForm.CustomerNote;

        ride.Status = RideStatus.Pending;
        // Save the ride
        var result = await _repo.RideRepository.Add(ride);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToCreateRide"));
        }

        var usersIsAdmin = await _repo.UserRepository.GetUsersIdsByRole(Roles.Admin);
        foreach (var adminId in usersIsAdmin)
        {
            string title = "طلب رحلة جديد ";
            string description = $" لديك طلب رحلة جديد  برقم {ride.RidingCode}" +
                                 $" من قبل العميل {customer.Username}";
            var userNotification = new UserNotificationForm
            {
                ReceiverId = adminId,
                Title = title,
                Description = description
            };
            await _notification.Create(userNotification);
        }

        return (_mapper.Map<RideDto>(result), null);
    }


    private async Task<string> GenerateRideCodeAsync()
    {
        var currentDate = DateTime.Now;
        var datePart = currentDate.ToString("yyyyMMdd");

        var lastRideToday = await _repo.RideRepository.GetLatestRideAsync(datePart);

        int nextSequenceNumber = 1;

        if (lastRideToday != null)
        {
            var lastRideCode = lastRideToday.RidingCode;
            var lastSequence = int.Parse(lastRideCode.Substring(8));
            nextSequenceNumber = lastSequence + 1;
        }

        return $"{datePart}{nextSequenceNumber:D5}";
    }

    public async Task<(RideDto? rideDto, string? error)> UpdateRideAsync(Guid id, RideUpdateForm rideUpdateForm)
    {
        var ride = await _repo.RideRepository.Get(x => x.Id == id,
            include: source => source
                .Include(x => x.Customer)
                .Include(x => x.Driver)
                .Include(x => x.Vehicle));

        if (ride == null)
        {
            return (null, _localize.GetLocalizedString("RideNotFound"));
        }

        if (!string.IsNullOrEmpty(rideUpdateForm.DiscountCode))
        {
            var discount = await _repo.DiscountRepository.Get(x => x.Code == rideUpdateForm.DiscountCode);
            if (discount != null &&
                discount.IsValidForRide(ride.CustomerId, DateTime.Now, ride.RideType))
            {
                // Apply the discount and calculate the final price
                ride.ApplyDiscount(discount);
                ride.DiscountId = discount.Id;
                ride.Discount = discount;
            }
            else
            {
                // If the discount code is invalid or not applicable, set the final price to the original price
                ride.FinalPrice = ride.Price;
            }
        }
        else
        {
            // If no discount code is provided, set the final price to the original price
            ride.FinalPrice = ride.Price;
        }

        if (rideUpdateForm.PickupTime != null)
        {
            if (rideUpdateForm.PickupTime < DateTime.Now)
            {
                return (null, _localize.GetLocalizedString("PickupTimeInThePast"));
            }

            ride.PickupTime = rideUpdateForm.PickupTime;
        }

        if (ride.Status == RideStatus.Pending || ride.Status == RideStatus.Confirmed)
        {
            if (rideUpdateForm.DriverId != null)
            {
                if (ride.DriverId != null)
                {
                    var currentDriver = await _repo.DriverRepository.Get(x => x.Id == ride.DriverId);
                    if (currentDriver != null)
                    {
                        currentDriver.Rides.Remove(ride);
                        ride.Driver = null;
                        ride.DriverId = null;
                        await _repo.DriverRepository.Update(currentDriver, currentDriver.Id);
                    }
                }

                var driver = await _repo.DriverRepository.Get(x => x.Id == rideUpdateForm.DriverId);
                if (driver == null)
                {
                    return (null, _localize.GetLocalizedString("DriverNotFound"));
                }

                if (driver.DriverStatus == DriverStatus.Suspended)
                {
                    return (null, _localize.GetLocalizedString("DriverNotAllowed"));
                }

                ride.DriverId = driver.Id;
                ride.Driver = driver;
                driver.Rides.Add(ride);
                await _repo.DriverRepository.Update(driver, driver.Id);

                var driverNotification = new UserNotificationForm
                {
                    ReceiverId = driver.Id,
                    Title = "طلب رحلة جديد",
                    Description = $"لديك طلب رحلة جديد برقم {ride.RidingCode}"
                };
                await _notification.CreateForDriver(driverNotification);
            }

            if (rideUpdateForm.VehicleId != null)
            {
                var currentVehicle = await _repo.VehiclesRepository.Get(x => x.Id == ride.VehicleId);
                if (currentVehicle != null)
                {
                    currentVehicle.Rides.Remove(ride);
                    ride.Vehicle = null;
                    ride.VehicleId = null;
                    await _repo.VehiclesRepository.Update(currentVehicle, currentVehicle.Id);
                }

                var vehicle = await _repo.VehiclesRepository.Get(x => x.Id == rideUpdateForm.VehicleId);
                if (vehicle == null)
                {
                    return (null, _localize.GetLocalizedString("VehicleNotFound"));
                }

                ride.VehicleId = vehicle.Id;
                ride.Vehicle = vehicle;
                vehicle.Rides.Add(ride);
                await _repo.VehiclesRepository.Update(vehicle, vehicle.Id);
            }
        }

        _mapper.Map(rideUpdateForm, ride);
        var result = await _repo.RideRepository.Update(ride, id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToUpdateRide"));
        }

        var dto = _mapper.Map<RideDto>(result);
        dto.CustomerName = result.Customer?.Username;
        dto.CustomerPhone = result.Customer?.PhoneNumber;
        dto.DriverName = result.Driver?.Username;
        dto.DriverPhone = result.Driver?.PhoneNumber;
        dto.VehicleName = result.Vehicle?.VehicleName;

        return (dto, null);
    }


    public async Task<(RideDto? rideDto, string? error)> GetRideByIdAsync(Guid id)
    {
        var ride = await _repo.RideRepository.Get(x => x.Id == id
            , include: source => source
                .Include(x => x.Customer)
                .Include(x => x.Driver)
                .Include(x => x.Vehicle)
                .Include(x => x.Attachments)
        );
        if (ride == null)
        {
            return (null, _localize.GetLocalizedString("RideNotFound"));
        }

        var dto = _mapper.Map<RideDto>(ride);
        dto = _dtoTranslationService.TranslateEnums(dto);
        return (dto, null);
    }

    public async Task<(List<RideDto>? rideDtos, int? totalCount, string? error)> GetAllRidesAsync(
        RideFilterForm filterForm)
    {
        var todayStart = DateTime.UtcNow.Date; // Use UTC date
        var todayEnd = todayStart.AddDays(1).AddTicks(-1); // End of the current UTC day

        var (Rides, totalCount) = await _repo.RideRepository.GetAll(
            x => (filterForm.CustomerId == null || x.CustomerId == filterForm.CustomerId) &&
                 (filterForm.RidingCode == null || x.RidingCode == filterForm.RidingCode) &&
                 (filterForm.DriverId == null || x.DriverId == filterForm.DriverId) &&
                 (filterForm.VehicleId == null || x.VehicleId == filterForm.VehicleId) &&
                 (filterForm.PickupLocation == null || x.PickupLocation == filterForm.PickupLocation) &&
                 (filterForm.DropOffLocation == null || x.DropOffLocation == filterForm.DropOffLocation) &&
                 (filterForm.IsDetour == null || x.IsDetour == filterForm.IsDetour) &&
                 (filterForm.PickupTime == null || x.PickupTime == filterForm.PickupTime) &&
                 (filterForm.CompletedAt == null || x.CompletedAt == filterForm.CompletedAt) &&
                 (!filterForm.IsToday.HasValue ||
                  (x.PickupTime >= todayStart &&
                   x.PickupTime <= todayEnd)) &&
                 !x.Deleted
            , include: source => source
                .Include(x => x.Customer)
                .Include(x => x.Driver)
                .Include(x => x.Vehicle)!,
            filterForm.PageNumber, filterForm.PageSize);

        var ridesDto = _mapper.Map<List<RideDto>>(Rides);
        ridesDto.ForEach(x => _dtoTranslationService.TranslateEnums(x));
        return (ridesDto, totalCount, null);
    }


    public async Task<(RideDto? rideDto, string? error)> DeleteRideAsync(Guid id)
    {
        var ride = await _repo.RideRepository.GetById(id);
        if (ride == null)
        {
            return (null, _localize.GetLocalizedString("RideNotFound"));
        }


        ride.Deleted = true;
        await _repo.RideRepository.Update(ride, id);

        return (null, null);
    }

    public async Task<(RideDto? rideDto, string? error)> ConfirmRideAsync(Guid id)
    {
        var ride = await _repo.RideRepository.Get(
            x => x.Id == id,
            include: source => source
                .Include(x => x.Customer)
                .Include(x => x.Driver)
                .Include(x => x.Vehicle)
        );
        if (ride == null)
        {
            return (null, _localize.GetLocalizedString("RideNotFound"));
        }

        if (ride.Status != RideStatus.Pending)
        {
            return (null, _localize.GetLocalizedString("RideNotPending"));
        }

        // Get the current user's ID from the claims
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return (null, _localize.GetLocalizedString("UserNotAuthenticated"));
        }

        var user = await _repo.UserRepository.GetById(Guid.Parse(userId));
        if (user == null || user.Role != Roles.Admin)
        {
            return (null, _localize.GetLocalizedString("UserNotAuthenticated"));
        }

        // Set the AcceptedByAdminId
        ride.AcceptedByAdminId = Guid.Parse(userId);
        // Use UTC time for DateTime values to avoid the PostgreSQL 'timestamp with time zone' error
        ride.AcceptedByAdminAt = DateTime.UtcNow.AddHours(3);
        ride.Status = RideStatus.Confirmed;

        var result = await _repo.RideRepository.Update(ride, id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToConfirmRide"));
        }

        var culture = _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? "en";

        if (!ride.Customer.IsEnglish)
        {
            string title = "تم قبول طلب الرحلة";
            string description = $"تم قبول طلب الرحلة الخاص بك من قبل الإدارة";
            var userNotification = new UserNotificationForm
            {
                ReceiverId = result.Customer.Id,
                Title = title,
                Description = description
            };
            await _notification.Create(userNotification);
        }
        else
        {
            string title = "Ride Request Accepted";
            string description = $"Your ride request has been accepted by the admin";
            var userNotification = new UserNotificationForm
            {
                ReceiverId = result.Customer.Id,
                Title = title,
                Description = description
            };
            await _notification.Create(userNotification);
        }


        return (_mapper.Map<RideDto>(result), null);
    }


    public async Task<(RideDto? rideDto, string? error)> RejectRideAsync(Guid id, string? rejectionReason)
    {
        var ride = await _repo.RideRepository.Get(x => x.Id == id,
            include: source => source
                .Include(x => x.Customer)
                .Include(x => x.Driver)
                .Include(x => x.Vehicle)
        );
        if (ride == null)
        {
            return (null, _localize.GetLocalizedString("RideNotFound"));
        }

        if (ride.Status != RideStatus.Pending)
        {
            return (null, _localize.GetLocalizedString("RideNotPending"));
        }

        // Get the current user's ID from the claims
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return (null, _localize.GetLocalizedString("UserNotAuthenticated"));
        }

        var user = await _repo.UserRepository.GetById(Guid.Parse(userId));
        if (user == null || user.Role != Roles.Admin)
        {
            return (null, _localize.GetLocalizedString("UserNotAuthenticated"));
        }

        // Set the RejectedByAdminId
        ride.RejectedByAdminId = Guid.Parse(userId);
        ride.RejectedByAdminAt = DateTime.UtcNow.AddHours(3); // Adjust time zone
        ride.Status = RideStatus.Rejected;
        ride.RejectionReason = rejectionReason;

        var result = await _repo.RideRepository.Update(ride, id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToRejectRide"));
        }

        string title = "Ride Request Rejected";
        string description = $"Your ride request has been rejected by the admin";

        var userNotification = new UserNotificationForm
        {
            ReceiverId = result.Customer.Id,
            Title = title,
            Description = description
        };
        await _notification.Create(userNotification);

        return (_mapper.Map<RideDto>(result), null);
    }


    public async Task<(RideDto? rideDto, string? error)> CancelRideAsync(Guid id, string? cancelationReason)
    {
        var ride = await _repo.RideRepository.GetById(id);
        if (ride == null)
        {
            return (null, _localize.GetLocalizedString("RideNotFound"));
        }

        if (ride.Status != RideStatus.Pending && ride.Status != RideStatus.Confirmed)
        {
            return (null, _localize.GetLocalizedString("RideNotPendingOrConfirmed"));
        }

        // Get the current user's ID from the claims
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return (null, _localize.GetLocalizedString("UserNotAuthenticated"));
        }

        var user = await _repo.UserRepository.GetById(Guid.Parse(userId));
        if (user == null && user.Role != Roles.Customer && user.Role != Roles.Admin)
        {
            return (null, _localize.GetLocalizedString("UserNotAuthenticated"));
        }

        if (user.Role == Roles.Customer)
        {
            ride.CanceledByCustomerAt = DateTime.UtcNow.AddHours(3); // Adjust time zone
            ride.IsCanceledByCustomer = true;
            ride.Status = RideStatus.Canceled;
            ride.CancelationReason = cancelationReason;
            var usersIsAdmin = await _repo.UserRepository.GetUsersIdsByRole(Roles.Admin);
            foreach (var adminId in usersIsAdmin)
            {
                string title = "Ride Request Canceled";
                string description =
                    $"Customer by {user.Username} has canceled the ride request with code {ride.RidingCode}";
                var userNotification = new UserNotificationForm
                {
                    ReceiverId = adminId,
                    Title = title,
                    Description = description
                };
                await _notification.Create(userNotification);
            }
        }

        if (user.Role == Roles.Admin)
        {
            ride.CanceledByAdminAt = DateTime.UtcNow.AddHours(3); // Adjust time zone
            ride.Status = RideStatus.Canceled;
            ride.CancelationReason = cancelationReason;


            if (!ride.Customer.IsEnglish)
            {
                string title = "تم إلغاء طلب الرحلة";
                string description = $"تم إلغاء طلب الرحلة برقم {ride.RidingCode} من قبل الإدارة";
                var userNotification = new UserNotificationForm
                {
                    ReceiverId = ride.CustomerId,
                    Title = title,
                    Description = description
                };
                await _notification.Create(userNotification);
            }
            else
            {
                string title = "Ride Request Canceled";
                string description = $"Admin has canceled the ride request with code {ride.RidingCode}";
                var userNotification = new UserNotificationForm
                {
                    ReceiverId = ride.CustomerId,
                    Title = title,
                    Description = description
                };
                await _notification.Create(userNotification);
            }
        }

        var result = await _repo.RideRepository.Update(ride, id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToCancelRide"));
        }


        return (_mapper.Map<RideDto>(result), null);
    }

    public async Task<(RideDto? rideDto, string? error)> StartRideAsync(Guid id)
    {
        var ride = await _repo.RideRepository.Get(
            x => x.Id == id,
            include: source => source
                .Include(x => x.Driver)
                .Include(x => x.Vehicle)
                .Include(x => x.Customer)
        );
        if (ride == null)
        {
            return (null, _localize.GetLocalizedString("RideNotFound"));
        }

        if (ride.Status != RideStatus.Confirmed)
        {
            return (null, _localize.GetLocalizedString("RideNotConfirmed"));
        }

        if (ride.Driver == null || ride.Vehicle == null)
        {
            return (null, _localize.GetLocalizedString("DriverOrVehicleNotFound"));
        }

        
        

        ride.StartedByDriverAt = DateTime.UtcNow.AddHours(3); // Adjust time zone
        ride.Status = RideStatus.Started;
        ride.Driver.DriverStatus = DriverStatus.OnRide;
        ride.Vehicle.VehicleStatus = VehicleStatus.OnTrip;

        var result = await _repo.RideRepository.Update(ride, id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToStartRide"));
        }

        if (!ride.Customer.IsEnglish)
        {
            string title = "بدأت الرحلة";
            string description = $"بدأ السائق الرحلة الخاصة بك";
            var userNotification = new UserNotificationForm
            {
                ReceiverId = ride.Customer.Id,
                Title = title,
                Description = description
            };
            await _notification.Create(userNotification);
        }
        else
        {
            string title = "Ride Started";
            string description = $"Your ride has been started by the driver";

            var userNotification = new UserNotificationForm
            {
                ReceiverId = ride.Customer.Id,
                Title = title,
                Description = description
            };

            await _notification.Create(userNotification);
        }

        return (_mapper.Map<RideDto>(result), null);
    }


    public async Task<(RideDto? rideDto, string? error)> CompleteRideAsync(Guid id)
    {
        var ride = await _repo.RideRepository.Get(
            x => x.Id == id,
            include: source => source
                .Include(x => x.Driver)
                .Include(x => x.Customer)
                .Include(x => x.Vehicle)
        );
        if (ride == null)
        {
            return (null, _localize.GetLocalizedString("RideNotFound"));
        }

        if (ride.Status != RideStatus.Started && ride.Status != RideStatus.ArrivedAtPickup &&
            ride.Status != RideStatus.ArrivedAtDetour)
        {
            return (null, _localize.GetLocalizedString("RideNotStarted"));
        }


        // var transaction = new Transaction
        // {
        //     Amount = ride.FinalPrice,
        //     RideId = ride.Id,
        //     Ride = ride
        // };
        // transaction.TransactionCode = ride.RidingCode + "-TR";
        //
        // if (ride.PaymentType == RidePaymentType.Cash)
        // {
        //     transaction.PaymentType = RidePaymentType.Cash;
        //     transaction.Status = TransactionPaymentStatus.Successful;
        // }
        // else
        // {
        //     transaction.PaymentType = RidePaymentType.Card;
        //     transaction.Status = TransactionPaymentStatus.Pending;
        // }
        //
        // if (ride.RideType == RideType.Normal)
        // {
        //     transaction.ServiceType = TransactionsServicesType.NormalRideService;
        // }
        // else
        // {
        //     transaction.ServiceType = TransactionsServicesType.VipRideService;
        // }
        //
        // await _repo.TransactionReposiotry.Add(transaction);

        ride.CompletedAt = DateTime.UtcNow.AddHours(3); // Adjust time zone
        ride.Status = RideStatus.Completed;
        ride.Driver.DriverStatus = DriverStatus.Available;

        if (ride.Driver.TripCount != null)
        {
            ride.Driver.TripCount++;
        }
        else
        {
            ride.Driver.TripCount = 1;
        }

        if (ride.Customer.TripCount != null)
        {
            ride.Customer.TripCount++;
        }
        else
        {
            ride.Customer.TripCount = 1;
        }

        ride.Vehicle.VehicleTripCount++;
        ride.Vehicle.VehicleStatus = VehicleStatus.Available;

        // Add payment logic after payment gateway integration
        ride.PaymentStatus = PaymentStatus.Paid;

        var result = await _repo.RideRepository.Update(ride, id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToCompleteRide"));
        }


        if (!ride.Customer.IsEnglish)
        {
            string title = "انتهت الرحلة";
            string description = $"انها السائق الرحلة الخاصة بك";
            var userNotification = new UserNotificationForm
            {
                ReceiverId = ride.Customer.Id,
                Title = title,
                Description = description
            };
            await _notification.Create(userNotification);
        }
        else
        {
            string title = "Ride Completed";
            string description = $"Your ride has been completed by the driver";

            var userNotification = new UserNotificationForm
            {
                ReceiverId = ride.Customer.Id,
                Title = title,
                Description = description
            };
            await _notification.Create(userNotification);
        }

        return (_mapper.Map<RideDto>(result), null);
    }

    public async Task<(RideDto? rideDto, string? error)> ArrivedAtPickupAsync(Guid id)
    {
        var ride = await _repo.RideRepository.Get(
            x => x.Id == id,
            include: source => source
                .Include(x => x.Driver)
                .Include(x => x.Customer)
                .Include(x => x.Vehicle)
        );

        if (ride == null)
        {
            return (null, _localize.GetLocalizedString("RideNotFound"));
        }

        if (ride.Status != RideStatus.Started)
        {
            return (null, _localize.GetLocalizedString("RideNotStarted"));
        }


        ride.Status = RideStatus.ArrivedAtPickup;

        var result = await _repo.RideRepository.Update(ride, id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToArrivedAtPickup"));
        }

        if (!ride.Customer.IsEnglish)
        {
            string title = "لقد وصل السائق";
            string description = $"لقد وصل السائق الى موقع المحدد";
            var userNotification = new UserNotificationForm
            {
                ReceiverId = ride.Customer.Id,
                Title = title,
                Description = description
            };
            await _notification.Create(userNotification);
        }
        else
        {
            string title = "Driver Arrived";
            string description = $"The driver has arrived at the pickup location";

            var userNotification = new UserNotificationForm
            {
                ReceiverId = ride.Customer.Id,
                Title = title,
                Description = description
            };
            await _notification.Create(userNotification);
        }


        return (_mapper.Map<RideDto>(result), null);
    }

    public async Task<(RideDto? rideDto, string? error)> ArrivedAtDetourAsync(Guid id)
    {
        var ride = await _repo.RideRepository.Get(
            x => x.Id == id,
            include: source => source
                .Include(x => x.Driver)
                .Include(x => x.Customer)
                .Include(x => x.Vehicle)
        );

        if (ride == null)
        {
            return (null, _localize.GetLocalizedString("RideNotFound"));
        }

        if (ride.Status != RideStatus.ArrivedAtPickup)
        {
            return (null, _localize.GetLocalizedString("RideNotArrivedAtPickup"));
        }

        ride.Status = RideStatus.ArrivedAtDetour;

        var result = await _repo.RideRepository.Update(ride, id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToArrivedAtDetour"));
        }


        if (!ride.Customer.IsEnglish)
        {
            string title = "لقد وصل السائق";
            string description = $"لقد وصل السائق الى الموقع الثاني";
            var userNotification = new UserNotificationForm
            {
                ReceiverId = ride.Customer.Id,
                Title = title,
                Description = description
            };
            await _notification.Create(userNotification);
        }
        else
        {
            string title = "Driver Arrived";
            string description = $"The driver has arrived at the detour location";

            var userNotification = new UserNotificationForm
            {
                ReceiverId = ride.Customer.Id,
                Title = title,
                Description = description
            };
            await _notification.Create(userNotification);
        }

        return (_mapper.Map<RideDto>(result), null);
    }

    public async Task<(RideDto? rideDto, string? error)> ChangeRideStatus(Guid id, RideStatus status, string? note)
    {
        var ride = await _repo.RideRepository.GetById(id);
        if (ride == null)
        {
            return (null, _localize.GetLocalizedString("RideNotFound"));
        }

        switch (status)
        {
            case RideStatus.Confirmed:
                return await ConfirmRideAsync(id);
            case RideStatus.Rejected:
                return await RejectRideAsync(id, note);
            case RideStatus.Canceled:
                return await CancelRideAsync(id, note);
            case RideStatus.Started:
                return await StartRideAsync(id);
            case RideStatus.Completed:
                return await CompleteRideAsync(id);
            case RideStatus.ArrivedAtPickup:
                return await ArrivedAtPickupAsync(id);
            case RideStatus.ArrivedAtDetour:
                return await ArrivedAtDetourAsync(id);
        }

        return (null, _localize.GetLocalizedString("InvalidRideStatus"));
    }

    public async Task<(int CompletedRidesPercentage, int CanceledRidesPercentage)> GetDriverRideStatistics(
        Guid driverId)
    {
        var totalRides = await _repo.RideRepository.Count(x => x.DriverId == driverId);
        var completedRides =
            await _repo.RideRepository.Count(x => x.DriverId == driverId && x.Status == RideStatus.Completed);
        var canceledRides =
            await _repo.RideRepository.Count(x => x.DriverId == driverId && x.Status == RideStatus.Canceled);
        var completedRidesPercentage = (int)Math.Round((decimal)((double)completedRides / totalRides * 100));
        var canceledRidesPercentage = (int)Math.Round((decimal)((double)canceledRides / totalRides * 100));
        return (completedRidesPercentage, canceledRidesPercentage);
    }
}