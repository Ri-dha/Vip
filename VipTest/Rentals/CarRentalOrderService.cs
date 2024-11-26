using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VipProjectV0._1.Db;
using VipTest.attachmentsConfig;
using VipTest.Localization;
using VipTest.Notifications;
using VipTest.Notifications.PayLoads;
using VipTest.Rentals.Dto;
using VipTest.Rentals.Models;
using VipTest.Rentals.PayLoads;
using VipTest.Rentals.utli;
using VipTest.Rides.Utli;
using VipTest.Transactions.models;
using VipTest.Transactions.utli;
using VipTest.Users.customers;
using VipTest.Users.Drivers;
using VipTest.Users.Models;
using VipTest.vehicles.Utli;

namespace VipTest.Rentals;

public interface ICarRentalOrderService
{
    Task<(CarRentalOrderDto? dto, string? error)> CreateAsync(CarRentalOrderCreateForm form);
    Task<(CarRentalOrderDto? dto, string? error)> UpdateAsync(Guid id, CarRentalOrderUpdateForm form);
    Task<(bool?, string? error)> DeleteAsync(Guid id);
    Task<(CarRentalOrderDto? dto, string? error)> GetByIdAsync(Guid id);
    Task<(List<CarRentalOrderDto>? dtos, int? totalCount, string? error)> GetListAsync(CarRentalOrderFilterForm form);
    Task<(CarRentalOrderDto? dto, string? error)> ConfirmAsync(Guid id);
    Task<(CarRentalOrderDto? dto, string? error)> RejectAsync(Guid id, string? rejectionReason);
    Task<(CarRentalOrderDto? dto, string? error)> CancelAsync(Guid id, string? reason);
    Task<(CarRentalOrderDto? dto, string? error)> PickedUpAsync(Guid id);

    Task<(CarRentalOrderDto? dto, string? error)> FinishAsync(Guid id);

    Task<(CarRentalOrderDto? dto, string? error)> ChangeStatusAsync(Guid id, RentalOrderStatus status, string? reason);
    Task<(CarRentalOrderDto? dto, string? error)> AssignDriverAsync(Guid id, Guid driverId);
}

public class CarRentalOrderService : ICarRentalOrderService
{
    private readonly IRepositoryWrapper _repo;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localize;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserNotificationServices _notification;
    private readonly IDtoTranslationService _dtoTranslationService;


    public CarRentalOrderService(IRepositoryWrapper repo, IMapper mapper, ILocalizationService localize,
        IHttpContextAccessor httpContextAccessor, IUserNotificationServices notification,
        IDtoTranslationService dtoTranslationService)
    {
        _repo = repo;
        _mapper = mapper;
        _localize = localize;
        _httpContextAccessor = httpContextAccessor;
        _notification = notification;
        _dtoTranslationService = dtoTranslationService;
    }

    public async Task<(CarRentalOrderDto? dto, string? error)> CreateAsync(CarRentalOrderCreateForm form)
    {
        var vehicle = await _repo.VehiclesRepository.Get(x => x.Id == form.VehicleId,
            include: source => source.Include(x => x.Warehouse).ThenInclude(x => x.BranchManager)
        );
        if (vehicle == null)
        {
            return (null, _localize.GetLocalizedString("VehicleNotFound"));
        }

        if (vehicle.VehicleStatus != VehicleStatus.Available)
        {
            return (null, _localize.GetLocalizedString("VehicleNotAvailable"));
        }

        if (vehicle.ForRent == false)
        {
            return (null, _localize.GetLocalizedString("VehicleNotForRent"));
        }

        if (form.PickupTime.Day < DateTime.UtcNow.Day)
        {
            return (null, _localize.GetLocalizedString("PickupTimeInThePast"));
        }

        if (form.DropOffTime < DateTime.UtcNow || form.DropOffTime < form.PickupTime)
        {
            return (null, _localize.GetLocalizedString("DropOffTimeInThePast"));
        }


        var rentOrder = _mapper.Map<CarRentalOrder>(form);

        rentOrder.PickupTime = form.PickupTime.ToUniversalTime();
        rentOrder.DropOffTime = form.DropOffTime.ToUniversalTime();
        rentOrder.PaymentStatus = PaymentStatus.UnPaid;
        rentOrder.PaymentType = RidePaymentType.Cash;
        rentOrder.OrderCode = await GenerateOrderCode();
        rentOrder.VehicleId = vehicle.Id;
        rentOrder.Vehicle = vehicle;
        rentOrder.WarehouseId = vehicle.WarehouseId;
        vehicle.CarRentalOrders.Add(rentOrder);
        rentOrder.CalculateRentDaysAndPrice(vehicle);

        if (form.PickUpType == PickUpType.FromWareHouse)
        {
            var destination = await _repo.WarehouseRepository.Get(x => x.Id == vehicle.WarehouseId);
            if (destination == null)
            {
                return (null, _localize.GetLocalizedString("WarehouseNotFound"));
            }

            rentOrder.PickupLocation = destination.WarehouseLocation;
            rentOrder.PickupLocationLatitude = (decimal)destination.WarehouseLocationLatitude;
            ;
            rentOrder.PickupLocationLongitude = (decimal)destination.WarehouseLocationLongitude;
        }
        else
        {
            rentOrder.PickupLocation = form.PickupLocation;
            rentOrder.PickupLocationLatitude = form.PickupLocationLatitude;
            rentOrder.PickupLocationLongitude = form.PickupLocationLongitude;
        }

        if (!string.IsNullOrEmpty(form.DiscountCode))
        {
            var discount = await _repo.DiscountRepository.Get(x => x.Code == form.DiscountCode);
            if (discount != null && discount.IsValidForCarRent(form.CustomerId, DateTime.Now))
            {
                rentOrder.ApplyDiscount(discount);
                rentOrder.DiscountId = discount.Id;
                rentOrder.Discount = discount;
            }
        }

        if (form.Attachments != null && form.Attachments.Count > 0)
        {
            rentOrder.Attachments = _mapper.Map<List<Attachments>>(form.Attachments);
        }

        var customer = await _repo.CustomerRepository.Get(x => x.Id == form.CustomerId);
        if (customer == null)
        {
            return (null, _localize.GetLocalizedString("CustomerNotFound"));
        }

        if(customer.CustomerStatus == CustomerStatus.Suspended)
        {
            return (null, _localize.GetLocalizedString("UserNotAllowed"));
        }
        rentOrder.CustomerId = customer.Id;
        rentOrder.Customer = customer;
        customer.CarRentalOrders.Add(rentOrder);


        var result = await _repo.CarRentalOrderRepository.Add(rentOrder);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToCreateCarRentalOrder"));
        }

        var warehouseNotification = new UserNotificationForm()
        {
            Title = "طلب حجز جديد",
            Description = $"تم إضافة طلب حجز جديد برقم {rentOrder.OrderCode}",
            ReceiverId = vehicle.Warehouse.BranchManagerId
        };

        await _notification.Create(warehouseNotification);

        return (_mapper.Map<CarRentalOrderDto>(result), null);
    }

    private async Task<string> GenerateOrderCode()
    {
        var datePart = DateTime.Now.ToString("yyyyMMdd");
        var latestOrder = await _repo.CarRentalOrderRepository.GetLatestCarRentalOrderAsync(datePart);
        if (latestOrder == null)
        {
            return datePart + "001";
        }

        var orderCode = latestOrder.OrderCode;
        var orderNumber = int.Parse(orderCode.Substring(orderCode.Length - 3));
        orderNumber++;
        return datePart + orderNumber.ToString().PadLeft(3, '0');
    }

    public async Task<(CarRentalOrderDto? dto, string? error)> UpdateAsync(Guid id, CarRentalOrderUpdateForm form)
    {
        var carRentalOrder = await _repo.CarRentalOrderRepository.Get(x => x.Id == id, include: source => source
            .Include(x => x.Vehicle)
            .Include(x => x.Customer)
            .Include(x => x.Driver));

        if (carRentalOrder == null)
        {
            return (null, _localize.GetLocalizedString("CarRentalOrderNotFound"));
        }

        if (carRentalOrder.Status == RentalOrderStatus.Pending || carRentalOrder.Status == RentalOrderStatus.Confirmed)
        {
            if (form.DriverId != null)
            {
                var driver = await _repo.DriverRepository.Get(x => x.Id == form.DriverId);
                if (driver == null)
                {
                    return (null, _localize.GetLocalizedString("DriverNotFound"));
                }
                if (driver.DriverStatus == DriverStatus.Suspended)
                {
                    return (null, _localize.GetLocalizedString("DriverNotAllowed"));
                }
                carRentalOrder.DriverId = driver.Id;
                carRentalOrder.Driver = driver;
                driver.CarRentalOrders.Add(carRentalOrder);
                await _repo.DriverRepository.Update(driver, driver.Id);
            }
        }

        _mapper.Map(form, carRentalOrder);
        var result = await _repo.CarRentalOrderRepository.Update(carRentalOrder, id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToUpdateCarRentalOrder"));
        }

        var dto = _mapper.Map<CarRentalOrderDto>(result);
        dto.DriverName = carRentalOrder.Driver != null ? carRentalOrder.Driver.Username : null;
        dto.DriverPhone = carRentalOrder.Driver != null ? carRentalOrder.Driver.PhoneNumber : null;
        dto.CustomerName = carRentalOrder.Customer.Username;
        dto.CustomerEmail = carRentalOrder.Customer.Email;
        dto.CustomerPhone = carRentalOrder.Customer.PhoneNumber;
        dto.VehicleName = carRentalOrder.Vehicle.VehicleName;
        dto.VehiclePlateNumber = carRentalOrder.Vehicle.VehicleLicensePlate;
        return (dto, null);
    }

    public async Task<(bool?, string? error)> DeleteAsync(Guid id)
    {
        var carRentalOrder = await _repo.CarRentalOrderRepository.Get(x => x.Id == id);
        if (carRentalOrder == null)
        {
            return (null, _localize.GetLocalizedString("CarRentalOrderNotFound"));
        }

        carRentalOrder.Deleted = true;
        await _repo.CarRentalOrderRepository.Update(carRentalOrder, id);

        return (true, null);
    }

    public async Task<(CarRentalOrderDto? dto, string? error)> GetByIdAsync(Guid id)
    {
        var carRentalOrder = await _repo.CarRentalOrderRepository.Get(x => x.Id == id, include: source => source
            .Include(x => x.Vehicle)
            .Include(x => x.Customer)
            .Include(x => x.Driver)
            .Include(x => x.Attachments)
        );
        if (carRentalOrder == null)
        {
            return (null, _localize.GetLocalizedString("CarRentalOrderNotFound"));
        }

        var dto = _mapper.Map<CarRentalOrderDto>(carRentalOrder);
        dto = _dtoTranslationService.TranslateEnums(dto);
        return (dto, null);
    }

    public async Task<(List<CarRentalOrderDto>? dtos, int? totalCount, string? error)> GetListAsync(
        CarRentalOrderFilterForm form)
    {
        var todayStart = DateTime.UtcNow.Date; // Use UTC date
        var todayEnd = todayStart.AddDays(1).AddTicks(-1); // End of the current UTC day
        var (carRentalOrders, totalCount) = await _repo.CarRentalOrderRepository.GetAll<CarRentalOrderDto>(
            x => (form.OrderCode == null || x.OrderCode.Contains(form.OrderCode))
                 && (form.VehicleId == null || x.VehicleId == form.VehicleId)
                 && (form.DriverId == null || x.DriverId == form.DriverId)
                 && (form.CustomerId == null || x.CustomerId == form.CustomerId)
                 && (form.Status == null || x.Status == form.Status)
                 && (form.PickupTime == null || x.PickupTime == form.PickupTime)
                 && (form.DropOffTime == null || x.DropOffTime == form.DropOffTime)
                 && (form.CarBackTime == null || x.CarBackTime == form.CarBackTime)
                 && (form.PickUpType == null || x.PickUpType == form.PickUpType)
                 && (form.IsToday == null ||
                     (form.IsToday == true && x.PickupTime >= todayStart && x.PickupTime <= todayEnd))
                 && (form.WarehouseId == null || x.WarehouseId == form.WarehouseId)
                 && x.Deleted == false,
            form.PageNumber, form.PageSize);

        carRentalOrders.ForEach(x => _dtoTranslationService.TranslateEnums(x));
        return (carRentalOrders, totalCount, null);
    }

    public async Task<(CarRentalOrderDto? dto, string? error)> ConfirmAsync(Guid id)
    {
        var carRentalOrder = await _repo.CarRentalOrderRepository.Get(
            x => x.Id == id, include: source => source
                .Include(x => x.Customer)
                .Include(x => x.Vehicle)
        );
        if (carRentalOrder == null)
        {
            return (null, _localize.GetLocalizedString("CarRentalOrderNotFound"));
        }

        if (carRentalOrder.Status != RentalOrderStatus.Pending)
        {
            return (null, _localize.GetLocalizedString("CarRentalOrderNotPending"));
        }

        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return (null, _localize.GetLocalizedString("UserNotFound"));
        }

        var user = await _repo.UserRepository.GetById(Guid.Parse(userId));
        // if (user == null || user.Role != Roles.Admin|| user.Role != Roles.BranchManager)
        // {
        //     return (null, _localize.GetLocalizedString("UserNotAuthenticated"));
        // }

        carRentalOrder.Status = RentalOrderStatus.Confirmed;
        carRentalOrder.AcceptedByAdminId = user?.Id;
        carRentalOrder.AcceptedByAdminAt = DateTime.UtcNow.AddHours(3);

        var result = await _repo.CarRentalOrderRepository.Update(carRentalOrder, id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToConfirmCarRentalOrder"));
        }

        var culture = _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? "en";
        if (!carRentalOrder.Customer.IsEnglish)
        {
            var userNotification = new UserNotificationForm()
            {
                Title = "تم قبول طلب الحجز",
                Description = $"تم قبول طلب الحجز الخاص بك برقم {carRentalOrder.OrderCode}",
                ReceiverId = carRentalOrder.Customer.Id
            };
            await _notification.Create(userNotification);
        }
        else
        {
            var userNotification = new UserNotificationForm()
            {
                Title = "Car Rental Order Confirmed",
                Description = $"Your car rental order with code {carRentalOrder.OrderCode} has been confirmed",
                ReceiverId = carRentalOrder.Customer.Id
            };
            await _notification.Create(userNotification);
        }

        return (_mapper.Map<CarRentalOrderDto>(result), null);
    }

    public async Task<(CarRentalOrderDto? dto, string? error)> RejectAsync(Guid id, string? rejectionReason)
    {
        var carRentalOrder = await _repo.CarRentalOrderRepository.Get(x => x.Id == id,
            include: source => source.Include(x => x.Customer)
                .Include(x => x.Vehicle)
                .ThenInclude(x => x.Warehouse)
                .ThenInclude(x => x.BranchManager)
        );
        if (carRentalOrder == null)
        {
            return (null, _localize.GetLocalizedString("CarRentalOrderNotFound"));
        }

        if (carRentalOrder.Status != RentalOrderStatus.Pending)
        {
            return (null, _localize.GetLocalizedString("CarRentalOrderNotPending"));
        }

        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return (null, _localize.GetLocalizedString("UserNotFound"));
        }

        var user = await _repo.UserRepository.GetById(Guid.Parse(userId));
        // if (user == null || user.Role != Roles.Admin|| user.Role != Roles.BranchManager)
        // {
        //     return (null, _localize.GetLocalizedString("UserNotAuthenticated"));
        // }

        carRentalOrder.Status = RentalOrderStatus.Rejected;
        carRentalOrder.RejectedByAdminId = user?.Id;
        carRentalOrder.RejectedByAdminAt = DateTime.UtcNow.AddHours(3);
        carRentalOrder.RejectionReason = rejectionReason;

        var result = await _repo.CarRentalOrderRepository.Update(carRentalOrder, id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToRejectCarRentalOrder"));
        }

        var culture = _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? "en";

        if (!carRentalOrder.Customer.IsEnglish)
        {
            var userNotification = new UserNotificationForm()
            {
                Title = "تم رفض طلب الحجز",
                Description = $"تم رفض طلب الحجز الخاص بك برقم {carRentalOrder.OrderCode}",
                ReceiverId = carRentalOrder.Customer.Id
            };
            await _notification.Create(userNotification);
        }
        else
        {
            var userNotification = new UserNotificationForm()
            {
                Title = "Car Rental Order Rejected",
                Description = $"Your car rental order with code {carRentalOrder.OrderCode} has been rejected",
                ReceiverId = carRentalOrder.Customer.Id
            };
            await _notification.Create(userNotification);
        }


        return (_mapper.Map<CarRentalOrderDto>(result), null);
    }

    public async Task<(CarRentalOrderDto? dto, string? error)> CancelAsync(Guid id, string? reason)
    {
        var carRentalOrder = await _repo.CarRentalOrderRepository.Get(x => x.Id == id,
            include: source => source.Include(x => x.Customer));

        if (carRentalOrder == null)
        {
            return (null, _localize.GetLocalizedString("CarRentalOrderNotFound"));
        }

        if (carRentalOrder.Status != RentalOrderStatus.Confirmed)
        {
            return (null, _localize.GetLocalizedString("CarRentalOrderNotConfirmed"));
        }

        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return (null, _localize.GetLocalizedString("UserNotFound"));
        }

        var user = await _repo.UserRepository.GetById(Guid.Parse(userId));
        // if (user == null || user.Role != Roles.Admin|| user.Role != Roles.Customer|| user.Role != Roles.BranchManager)
        // {
        //     return (null, _localize.GetLocalizedString("UserNotAuthenticated"));
        // }

        carRentalOrder.Status = RentalOrderStatus.Canceled;
        carRentalOrder.CanceledByAdminId = user?.Id;
        carRentalOrder.CanceledByAdminAt = DateTime.UtcNow.AddHours(3);
        carRentalOrder.CancelationReason = reason;

        var result = await _repo.CarRentalOrderRepository.Update(carRentalOrder, id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToCancelCarRentalOrder"));
        }

        var culture = _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? "en";

        if (!carRentalOrder.Customer.IsEnglish)
        {
            var userNotification = new UserNotificationForm()
            {
                Title = "تم إلغاء طلب الحجز",
                Description = $"تم إلغاء طلب الحجز الخاص بك برقم {carRentalOrder.OrderCode}",
                ReceiverId = carRentalOrder.CustomerId
            };
            await _notification.Create(userNotification);
        }
        else
        {
            var title = "Car Rental Order Canceled";
            var description = $"Your car rental order with code {carRentalOrder.OrderCode} has been canceled";
            var userNotification = new UserNotificationForm()
            {
                Title = title,
                Description = description,
                ReceiverId = carRentalOrder.CustomerId
            };
            await _notification.Create(userNotification);
        }

        return (_mapper.Map<CarRentalOrderDto>(result), null);
    }

    public async Task<(CarRentalOrderDto? dto, string? error)> FinishAsync(Guid id)
    {
        var carRentalOrder = await _repo.CarRentalOrderRepository.Get(x => x.Id == id,
            include: source => source.Include(x => x.Customer)
                .Include(x => x.Vehicle)
                .ThenInclude(x => x.Warehouse)
                .ThenInclude(x => x.BranchManager)
        );
        if (carRentalOrder == null)
        {
            return (null, _localize.GetLocalizedString("CarRentalOrderNotFound"));
        }

        // var transaction = new Transaction
        // {
        //     Amount = carRentalOrder.FinalPrice,
        //     CarRentalId = carRentalOrder.Id,
        //     CarRental = carRentalOrder,
        // };
        // transaction.TransactionCode = carRentalOrder.OrderCode + "-TR";
        // transaction.WarehouseId = carRentalOrder.WarehouseId;
        // transaction.ServiceType = TransactionsServicesType.CarRentalService;
        //
        // if (carRentalOrder.PaymentType == RidePaymentType.Cash)
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
        // transaction.CarRental = carRentalOrder;
        // transaction.CarRentalId = carRentalOrder.Id;
        // await _repo.TransactionReposiotry.Add(transaction);
        carRentalOrder.Status = RentalOrderStatus.Completed;

        var customer = carRentalOrder.Customer;

        if (customer.RentCount != null)
        {
            customer.RentCount++;
        }
        else
        {
            customer.RentCount = 1;
        }

        customer.RentCount++;
        await _repo.CustomerRepository.Update(customer, customer.Id);

        var result = await _repo.CarRentalOrderRepository.Update(carRentalOrder, id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToFinishCarRentalOrder"));
        }

        var culture = _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? "en";

        if (!carRentalOrder.Customer.IsEnglish)
        {
            var userNotification = new UserNotificationForm()
            {
                Title = "تم إتمام طلب الحجز",
                Description = $"تم إتمام طلب الحجز الخاص بك برقم {carRentalOrder.OrderCode}",
                ReceiverId = carRentalOrder.Customer.Id
            };
            await _notification.Create(userNotification);
        }
        else
        {
            var userNotification = new UserNotificationForm()
            {
                Title = "Car Rental Order Completed",
                Description = $"Your car rental order with code {carRentalOrder.OrderCode} has been completed",
                ReceiverId = carRentalOrder.Customer.Id
            };

            await _notification.Create(userNotification);
        }

        return (_mapper.Map<CarRentalOrderDto>(result), null);
    }

    public async Task<(CarRentalOrderDto? dto, string? error)> PickedUpAsync(Guid id)
    {
        var carRentalOrder = await _repo.CarRentalOrderRepository.Get(x => x.Id == id,
            include: source => source.Include(x => x.Vehicle)
                .ThenInclude(x => x.Warehouse)
                .ThenInclude(x => x.BranchManager)
                .Include(X => X.Customer)
        );
        if (carRentalOrder == null)
        {
            return (null, _localize.GetLocalizedString("CarRentalOrderNotFound"));
        }

        if (carRentalOrder.Status != RentalOrderStatus.Confirmed)
        {
            return (null, _localize.GetLocalizedString("CarRentalOrderNotConfirmed"));
        }


        carRentalOrder.Status = RentalOrderStatus.PickedUp;
        carRentalOrder.PickupTime = DateTime.UtcNow.AddHours(3);
        carRentalOrder.Vehicle.VehicleStatus = VehicleStatus.NotAvailable;


        var result = await _repo.CarRentalOrderRepository.Update(carRentalOrder, id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToPickedUpCarRentalOrder"));
        }

        var userNotification = new UserNotificationForm()
        {
            Title = "طلب الحجز تم تسليمه",
            Description = $"تم تسليم طلب الحجز الخاص بك برقم {carRentalOrder.OrderCode}",
            ReceiverId = carRentalOrder.Vehicle.Warehouse.BranchManager.Id
        };
        await _notification.Create(userNotification);

        return (_mapper.Map<CarRentalOrderDto>(result), null);
    }

    // public async Task<(CarRentalOrderDto? dto, string? error)> DropOffCodeAsync(Guid id)
    // {
    //     var carRentalOrder = await _repo.CarRentalOrderRepository.Get(x => x.Id == id);
    //     if (carRentalOrder == null)
    //     {
    //         return (null, _localize.GetLocalizedString("CarRentalOrderNotFound"));
    //     }
    //
    //     if (carRentalOrder.Status != RentalOrderStatus.PickedUp)
    //     {
    //         return (null, _localize.GetLocalizedString("CarRentalOrderNotPickedUp"));
    //     }
    //
    //     carRentalOrder.Status = RentalOrderStatus.DroppedOff;
    //     carRentalOrder.DropOffTime = DateTime.UtcNow.AddHours(3);
    //     carRentalOrder.Vehicle.VehicleStatus = VehicleStatus.Available;
    //
    //     var result = await _repo.CarRentalOrderRepository.Update(carRentalOrder, id);
    //     if (result == null)
    //     {
    //         return (null, _localize.GetLocalizedString("FailedToDropOffCarRentalOrder"));
    //     }
    //
    //     return (_mapper.Map<CarRentalOrderDto>(result), null);
    // }

    // public async Task<(CarRentalOrderDto? dto, string? error)> CarBackAsync(Guid id)
    // {
    //     var carRentalOrder = await _repo.CarRentalOrderRepository.Get(x => x.Id == id);
    //     if (carRentalOrder == null)
    //     {
    //         return (null, _localize.GetLocalizedString("CarRentalOrderNotFound"));
    //     }
    //
    //     if (carRentalOrder.Status != RentalOrderStatus.DroppedOff)
    //     {
    //         return (null, _localize.GetLocalizedString("CarRentalOrderNotDroppedOff"));
    //     }
    //
    //     carRentalOrder.Status = RentalOrderStatus.CarBack;
    //     carRentalOrder.CarBackTime = DateTime.UtcNow.AddHours(3);
    //     carRentalOrder.Vehicle.VehicleStatus = VehicleStatus.Available;
    //
    //     var result = await _repo.CarRentalOrderRepository.Update(carRentalOrder, id);
    //     if (result == null)
    //     {
    //         return (null, _localize.GetLocalizedString("FailedToCarBackCarRentalOrder"));
    //     }
    //
    //     return (_mapper.Map<CarRentalOrderDto>(result), null);
    // }

    public async Task<(CarRentalOrderDto? dto, string? error)> ChangeStatusAsync(Guid id, RentalOrderStatus status,
        string? reason)
    {
        var carRentalOrder = await _repo.CarRentalOrderRepository.Get(x => x.Id == id);
        if (carRentalOrder == null)
        {
            return (null, _localize.GetLocalizedString("CarRentalOrderNotFound"));
        }

        switch (status)
        {
            case RentalOrderStatus.Canceled:
                return await CancelAsync(id, reason);
            case RentalOrderStatus.Confirmed:
                return await ConfirmAsync(id);
            case RentalOrderStatus.Rejected:
                return await RejectAsync(id, reason);
            case RentalOrderStatus.Completed:
                return await FinishAsync(id);
            case RentalOrderStatus.PickedUp:
                return await PickedUpAsync(id);
        }

        return (null, _localize.GetLocalizedString("InvalidStatus"));
    }

    public async Task<(CarRentalOrderDto? dto, string? error)> AssignDriverAsync(Guid id, Guid driverId)
    {
        var carRentalOrder = await _repo.CarRentalOrderRepository.Get(x => x.Id == id);
        if (carRentalOrder == null)
        {
            return (null, _localize.GetLocalizedString("CarRentalOrderNotFound"));
        }

        var driver = await _repo.DriverRepository.Get(x => x.Id == driverId);
        if (driver == null)
        {
            return (null, _localize.GetLocalizedString("DriverNotFound"));
        }

        carRentalOrder.DriverId = driver.Id;
        carRentalOrder.Driver = driver;
        driver.CarRentalOrders.Add(carRentalOrder);

        var result = await _repo.CarRentalOrderRepository.Update(carRentalOrder, id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToAssignDriver"));
        }

        return (_mapper.Map<CarRentalOrderDto>(result), null);
    }
}