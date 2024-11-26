using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VipProjectV0._1.Db;
using VipTest.attachmentsConfig;
using VipTest.Localization;
using VipTest.Notifications;
using VipTest.Notifications.Models;
using VipTest.Notifications.PayLoads;
using VipTest.Users.Models;
using VipTest.vehicles.Dtos;
using VipTest.vehicles.Modles;
using VipTest.vehicles.PayLoads;
using VipTest.vehicles.Utli;

namespace VipTest.vehicles.Services;

public interface IRentCarsServices
{
    Task<(RentCarsDto? rentCarsDto, string? error)> CreateRentCarAsync(RentCarsCreateForm rentCarsCreateForm);
    Task<(RentCarsDto? rentCarsDto, string? error)> UpdateRentCarAsync(Guid id, RentCarsUpdateForm rentCarsUpdateForm);
    Task<(RentCarsDto? rentCarsDto, string? error)> GetRentCarByIdAsync(Guid id);

    Task<(List<RentCarsDto>? rentCarsDtos, int? totalCount, string? error)> GetAllRentCarsAsync(
        VehicleFilterForm filterForm);

    Task<(bool, string? error)> DeleteRentCarAsync(Guid id);
    Task<(bool, string? error)> FavoriteRentCarAsync(Guid id, Guid userId);
    Task<(List<RentCarsDto>? dtos, string? error)> GetFavoriteRentCarsAsync(Guid userId);
}

public class RentCarsServices : IRentCarsServices
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localize;
    private readonly IUserNotificationServices _notification;
    private readonly IDtoTranslationService _dtoTranslationService;


    public RentCarsServices(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILocalizationService localize,
        IUserNotificationServices notification, IDtoTranslationService dtoTranslationService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _localize = localize;
        _notification = notification;
        _dtoTranslationService = dtoTranslationService;
    }

    public async Task<(RentCarsDto? rentCarsDto, string? error)> CreateRentCarAsync(
        RentCarsCreateForm rentCarsCreateForm)
    {
        var settings = await _repositoryWrapper.SettingsRepository.Get(x => true);
        if (settings == null)
        {
            return (null, _localize.GetLocalizedString("SettingsNotFound"));
        }

        var rentCar = _mapper.Map<Vehicles>(rentCarsCreateForm);

        // Add attachments if provided
        if (rentCarsCreateForm.Attachments != null && rentCarsCreateForm.Attachments.Count > 0)
        {
            rentCar.Attachments = _mapper.Map<List<Attachments>>(rentCarsCreateForm.Attachments);
        }

        if (rentCarsCreateForm.VehicleImages != null && rentCarsCreateForm.VehicleImages.Count > 0)
        {
            rentCar.VehicleImages = rentCarsCreateForm.VehicleImages;
        }

        rentCar.IsInWarehouse = true;
        rentCar.ForRent = true;
        rentCar.CarAcceptanceStatus = CarAcceptanceStatus.Pending;
        rentCar.VehicleStatus = VehicleStatus.NotAvailable;
        rentCar.RentalPriceUsd = rentCarsCreateForm.RentalPrice * settings.IqdToUsd;

        if (rentCarsCreateForm.HasDrivers != null)
        {
            rentCar.HasDrivers = rentCarsCreateForm.HasDrivers;
        }
        else
        {
            rentCar.HasDrivers = false;
        }
        
        var warehouse = await _repositoryWrapper.WarehouseRepository.Get(x => rentCarsCreateForm.WarehouseId == x.Id);
        if (warehouse == null)
        {
            return (null, _localize.GetLocalizedString("WarehouseNotFound"));
        }

        rentCar.Warehouse = warehouse;
        rentCar.WarehouseId = warehouse.Id;
        warehouse.WarehouseVehicles.Add(rentCar);

        var result = await _repositoryWrapper.VehiclesRepository.Add(rentCar);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToCreateRentCar"));
        }

        var usersIsAdmin = await _repositoryWrapper.UserRepository.GetUsersIdsByRole(Roles.Admin);
        foreach (var adminId in usersIsAdmin)
        {
            var title = "تم اضافة سيارة أيجار جديدة";
            var description = $" تم اضافة سيارة أيجار جديدة بنجاح بواسطة {rentCar.Warehouse.WarehouseName}";
            var userNotification = new UserNotificationForm
            {
                ReceiverId = adminId,
                Title = title,
                Description = description
            };
            await _notification.Create(userNotification);
        }

        return (_mapper.Map<RentCarsDto>(result), null);
    }

    public async Task<(RentCarsDto? rentCarsDto, string? error)> UpdateRentCarAsync(Guid id, RentCarsUpdateForm form)
    {
        var settings = await _repositoryWrapper.SettingsRepository.Get(x => true);
        if (settings == null)
        {
            return (null, _localize.GetLocalizedString("SettingsNotFound"));
        }

        var rentCar = await _repositoryWrapper.VehiclesRepository.Get(x => x.Id == id,
            include: source => source.Include(x => x.Attachments)
                .Include(x => x.Warehouse));

        if (rentCar == null)
        {
            return (null, _localize.GetLocalizedString("RentCarNotFound"));
        }


        if (form.RentalPrice != null)
        {
            rentCar.RentalPrice = form.RentalPrice;
            rentCar.RentalPriceUsd = form.RentalPrice * settings.IqdToUsd;
        }

        // Update attachments if provided
        if (form.Attachments != null)
        {
            rentCar.Attachments.RemoveAll(x => x.Id != null);
            rentCar.Attachments = _mapper.Map<List<Attachments>>(form.Attachments);
        }

        if (form.CarAcceptanceStatus != null && form.CarAcceptanceStatus == CarAcceptanceStatus.Accepted)
        {
            rentCar.VehicleStatus = VehicleStatus.Available;
            rentCar.CarAcceptanceStatus = CarAcceptanceStatus.Accepted;

            var warehouseManagerNotification = new UserNotificationForm
            {
                Title = "تغير بحالة السيارة",
                Description = $"تم قبول السيارة {rentCar.VehicleName} ",
                ReceiverId = rentCar.Warehouse.BranchManagerId
            };
            await _notification.Create(warehouseManagerNotification);
        }
        
        if (form.HasDrivers != null)
        {
            rentCar.HasDrivers = form.HasDrivers;
        }
        else
        {
            rentCar.HasDrivers = false;
        }

        if (form.CarAcceptanceStatus != null && form.CarAcceptanceStatus == CarAcceptanceStatus.Pending)
        {
            rentCar.VehicleStatus = VehicleStatus.NotAvailable;
            rentCar.CarAcceptanceStatus = CarAcceptanceStatus.Pending;

            var warehouseManagerNotification = new UserNotificationForm
            {
                Title = "تغير بحالة السيارة",
                Description = $"تم تعليق السيارة {rentCar.VehicleName} ",
                ReceiverId = rentCar.Warehouse.BranchManagerId
            };
            await _notification.Create(warehouseManagerNotification);
        }


        if (form.CarAcceptanceStatus != null && form.CarAcceptanceStatus == CarAcceptanceStatus.Rejected)
        {
            rentCar.VehicleStatus = VehicleStatus.NotAvailable;
            rentCar.CarAcceptanceStatus = CarAcceptanceStatus.Rejected;

            if (string.IsNullOrEmpty(form.Note))
            {
                rentCar.Note = form.Note;
            }

            var warehouseManagerNotification = new UserNotificationForm
            {
                Title = "تغير بحالة السيارة",
                Description = $"تم رفض السيارة {rentCar.VehicleName} بسبب {form.Note}",
                ReceiverId = rentCar.Warehouse.BranchManagerId
            };
            await _notification.Create(warehouseManagerNotification);
        }

        if (form.CarAcceptanceStatus != null)
        {
        }

        if (string.IsNullOrEmpty(form.Note))
        {
            rentCar.Note = form.Note;
        }

        if (form.VehicleImages != null && form.VehicleImages.Count > 0)
        {
            rentCar.VehicleImages = form.VehicleImages;
        }

        if (string.IsNullOrEmpty(form.GeneralDescription))
        {
            rentCar.GeneralDescription = form.GeneralDescription;
        }


        _mapper.Map(form, rentCar);

        var result = await _repositoryWrapper.VehiclesRepository.Update(rentCar, id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToUpdateRentCar"));
        }

        return (_mapper.Map<RentCarsDto>(result), null);
    }

    public async Task<(RentCarsDto? rentCarsDto, string? error)> GetRentCarByIdAsync(Guid id)
    {
        var result = await _repositoryWrapper.VehiclesRepository.Get(
            x => x.Id == id,
            include: query => query
                .Include(v => v.Warehouse)
                .Include(v => v.Attachments)
                .Include(v => v.CarRentalOrders));

        if (result == null)
        {
            return (null, _localize.GetLocalizedString("RentCarNotFound"));
        }

        var rentCarDto = _mapper.Map<RentCarsDto>(result);
        rentCarDto = _dtoTranslationService.TranslateEnums(rentCarDto);
        return (rentCarDto, null);
    }

    public async Task<(List<RentCarsDto>? rentCarsDtos, int? totalCount, string? error)> GetAllRentCarsAsync(
        VehicleFilterForm filterForm)
    {
        var filterExpression = BuildFilterExpression(filterForm);
        var (vehicles, totalCount) = await _repositoryWrapper.VehiclesRepository.GetAll<RentCarsDto>(
            filterExpression, filterForm.PageNumber, filterForm.PageSize);

        if (filterForm.UserId != null)
        {
            var customer = await _repositoryWrapper.CustomerRepository.Get(
                x => x.Id == filterForm.UserId,
                include: query => query.Include(x => x.FavoriteVehicles)
            );
            if (customer == null)
            {
                return (null, null, _localize.GetLocalizedString("CustomerNotFound"));
            }

            var favoriteVehicleIds = customer.FavoriteVehicles.Select(v => v.Id).ToHashSet();
            vehicles.ForEach(vehicle => vehicle.IsFavourite = favoriteVehicleIds.Contains(vehicle.Id));
        }

        vehicles.ForEach(vehicle => _dtoTranslationService.TranslateEnums(vehicle));
        return (vehicles, totalCount, null);
    }

// Builds a filter expression based on the filterForm
    private static Expression<Func<Vehicles, bool>> BuildFilterExpression(VehicleFilterForm filterForm)
    {
        return x =>
                (string.IsNullOrEmpty(filterForm.VehicleName) || x.VehicleName.Contains(filterForm.VehicleName)) &&
                (string.IsNullOrEmpty(filterForm.VehicleModel) || x.VehicleModel.Contains(filterForm.VehicleModel)) &&
                (string.IsNullOrEmpty(filterForm.VehicleNumber) ||
                 x.VehicleNumber.Contains(filterForm.VehicleNumber)) &&
                (filterForm.VehicleType == null || x.VehicleType == filterForm.VehicleType) &&
                (filterForm.VehicleStatus == null || x.VehicleStatus == filterForm.VehicleStatus) &&
                (filterForm.WarehouseId == null || x.WarehouseId == filterForm.WarehouseId) &&
                (filterForm.WarehouseName == null || x.Warehouse.WarehouseName == filterForm.WarehouseName) &&
                (filterForm.VehicleCapacity == null || x.VehicleCapacity == filterForm.VehicleCapacity) &&
                (filterForm.VehicleRating == null || x.VehicleRating == filterForm.VehicleRating) &&
                (filterForm.VehicleColor == null || x.VehicleColor.Contains(filterForm.VehicleColor)) &&
                (filterForm.VehicleYear == null || x.VehicleYear == filterForm.VehicleYear) &&
                (string.IsNullOrEmpty(filterForm.VehicleLicensePlate) ||
                 x.VehicleLicensePlate.Contains(filterForm.VehicleLicensePlate)) &&
                (string.IsNullOrEmpty(filterForm.VehicleRegistration) ||
                 x.VehicleRegistration.Contains(filterForm.VehicleRegistration)) &&
                (filterForm.VehicleBrand == null || x.VehicleBrand == filterForm.VehicleBrand) &&
                (filterForm.CarType == null || x.CarType == filterForm.CarType) &&
                (filterForm.ShifterType == null || x.ShifterType == filterForm.ShifterType) &&
                (filterForm.StartRentalPrice == null || x.RentalPrice >= filterForm.StartRentalPrice) &&
                (filterForm.EndRentalPrice == null || x.RentalPrice <= filterForm.EndRentalPrice) &&
                (filterForm.StartYear == null || x.VehicleYear >= filterForm.StartYear) &&
                (filterForm.EndYear == null || x.VehicleYear <= filterForm.EndYear) &&
                (filterForm.HasRentalPrice == null || x.RentalPrice != null) &&
                (filterForm.CarAcceptanceStatus == null || x.CarAcceptanceStatus == filterForm.CarAcceptanceStatus) &&
                x.ForRent == true &&
                x.Deleted == false
            ;
    }


    public async Task<(bool, string? error)> DeleteRentCarAsync(Guid id)
    {
        var car = await _repositoryWrapper.VehiclesRepository.Get(x => x.Id == id);
        if (car == null)
        {
            return (false, _localize.GetLocalizedString("RentCarNotFound"));
        }

        car.Deleted = true;

        await _repositoryWrapper.VehiclesRepository.Update(car, id);
        return (true, null);
    }

    public async Task<(bool, string? error)> FavoriteRentCarAsync(Guid id, Guid userId)
    {
        var car = await _repositoryWrapper.VehiclesRepository.Get(x => x.Id == id);
        if (car == null)
        {
            return (false, _localize.GetLocalizedString("RentCarNotFound"));
        }

        var user = await _repositoryWrapper.CustomerRepository.Get(x => x.Id == userId);
        if (user == null)
        {
            return (false, _localize.GetLocalizedString("UserNotFound"));
        }

        if (user.FavoriteVehicles.Contains(car))
        {
            user.FavoriteVehicles.Remove(car);

            var result = await _repositoryWrapper.UserRepository.Update(user, userId);
            if (result == null)
            {
                return (false, _localize.GetLocalizedString("FailedToFavoriteRentCar"));
            }

            return (false, null);
        }
        else
        {
            user.FavoriteVehicles.Add(car);
            var result = await _repositoryWrapper.UserRepository.Update(user, userId);
            if (result == null)
            {
                return (false, _localize.GetLocalizedString("FailedToFavoriteRentCar"));
            }

            return (true, null);
        }
    }

    public async Task<(List<RentCarsDto>? dtos, string? error)> GetFavoriteRentCarsAsync(Guid userId)
    {
        var user = await _repositoryWrapper.CustomerRepository.Get(x => x.Id == userId,
            include: query => query.Include(x => x.FavoriteVehicles)
                .ThenInclude(x => x.Warehouse)
        );
        if (user == null)
        {
            return (null, _localize.GetLocalizedString("UserNotFound"));
        }

        var vehicles = user.FavoriteVehicles;
        var dtos = _mapper.Map<List<RentCarsDto>>(vehicles);
        dtos.ForEach(vehicle => _dtoTranslationService.TranslateEnums(vehicle));
        var favoriteVehicleIds = user.FavoriteVehicles.Select(v => v.Id).ToHashSet();
        dtos.ForEach(vehicle => vehicle.IsFavourite = favoriteVehicleIds.Contains(vehicle.Id));
        return (dtos, null);
    }
}