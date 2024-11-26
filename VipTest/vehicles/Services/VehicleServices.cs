using System.Net.Mail;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VipProjectV0._1.Db;
using VipTest.attachmentsConfig;
using VipTest.Localization;
using VipTest.Rides.Dto;
using VipTest.vehicles.Dtos;
using VipTest.vehicles.Modles;
using VipTest.vehicles.PayLoads;

namespace VipTest.vehicles.Services;

public interface IVehicleServices
{
    Task<(VehiclesDto? vehicleDto, string? error)> CreateVehicleAsync(VehicleCreateForm vehicleDto);
    Task<(VehiclesDto? vehicleDto, string? error)> UpdateVehicleAsync(Guid id, VehicleUpdateForm vehicleUpdateForm);
    Task<(VehiclesDto? vehicleDto, string? error)> GetVehicleByIdAsync(Guid id);

    Task<(List<VehiclesDto>? vehicleDtos, int? totalCount, string? error)> GetAllVehiclesAsync(
        VehicleFilterForm filterForm);

    Task<(bool?, string? error)> DeleteVehicleAsync(Guid id);
    Task<(bool, string? error)> FavoriteVehicleAsync(Guid id, Guid userId);
}

public class VehicleServices : IVehicleServices
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ILogger<VehicleServices> _logger;
    private readonly ILocalizationService _localize;
    private readonly IDtoTranslationService _dtoTranslationService;


    public VehicleServices(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILogger<VehicleServices> logger,
        ILocalizationService localize, IDtoTranslationService dtoTranslationService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _logger = logger;
        _localize = localize;
        _dtoTranslationService = dtoTranslationService;
    }

    public async Task<(VehiclesDto? vehicleDto, string? error)> CreateVehicleAsync(VehicleCreateForm createForm)
    {
        var vehicle = _mapper.Map<Vehicles>(createForm);

        vehicle.IsInWarehouse = false;

        // Add attachments if provided
        if (createForm.Attachments != null && createForm.Attachments.Count > 0)
        {
            vehicle.Attachments = _mapper.Map<List<Attachments>>(createForm.Attachments);
        }

        var result = await _repositoryWrapper.VehiclesRepository.Add(vehicle);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToCreateVehicle"));
        }

        return (_mapper.Map<VehiclesDto>(result), null);
    }

    public async Task<(VehiclesDto? vehicleDto, string? error)> UpdateVehicleAsync(Guid id,
        VehicleUpdateForm vehicleUpdateForm)
    {
        var vehicle = await _repositoryWrapper.VehiclesRepository.Get(x => x.Id == id,
            include: query => query.Include(v => v.Warehouse).Include(v => v.Rides).Include(v => v.Attachments)
            );
        if (vehicle == null)
        {
            return (null, _localize.GetLocalizedString("VehicleNotFound"));
        }


        // Update attachments if provided
        if (vehicleUpdateForm.Attachments != null)
        {
            vehicle.Attachments.RemoveAll(x => x.Id != null);
            vehicle.Attachments = _mapper.Map<List<Attachments>>(vehicleUpdateForm.Attachments);
        }

        _mapper.Map(vehicleUpdateForm, vehicle);
        var result = await _repositoryWrapper.VehiclesRepository.Update(vehicle, id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToUpdateVehicle"));
        }

        return (_mapper.Map<VehiclesDto>(result), null);
    }

    public async Task<(VehiclesDto? vehicleDto, string? error)> GetVehicleByIdAsync(Guid id)
    {
        var result = await _repositoryWrapper.VehiclesRepository.Get(
            x => x.Id == id,
            include: query => query
                .Include(v => v.Warehouse)
                .Include(v => v.Rides)
                .Include(v => v.Attachments));

        if (result == null)
        {
            return (null, _localize.GetLocalizedString("VehicleNotFound"));
        }


        var vehicleDto = _mapper.Map<VehiclesDto>(result);
        vehicleDto = _dtoTranslationService.TranslateEnums(vehicleDto);

        return (vehicleDto, null);
    }

    public async Task<(List<VehiclesDto>? vehicleDtos, int? totalCount, string? error)> GetAllVehiclesAsync(
        VehicleFilterForm filterForm)
    {
        var (vehicles, totalCount) = await _repositoryWrapper.VehiclesRepository.GetAll(
            x => (string.IsNullOrEmpty(filterForm.VehicleName) || x.VehicleName.Contains(filterForm.VehicleName)) &&
                 (string.IsNullOrEmpty(filterForm.VehicleModel) || x.VehicleModel.Contains(filterForm.VehicleModel)) &&
                 (string.IsNullOrEmpty(filterForm.VehicleNumber) ||
                  x.VehicleNumber.Contains(filterForm.VehicleNumber)) &&
                 (filterForm.VehicleType == null || x.VehicleType == filterForm.VehicleType) &&
                 (filterForm.VehicleStatus == null || x.VehicleStatus == filterForm.VehicleStatus) &&
                 (filterForm.WarehouseId == null || x.WarehouseId == filterForm.WarehouseId) &&
                 
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
                 (filterForm.HasRentalPrice == null || x.RentalPrice != null) &&
                 
                 x.IsInWarehouse == false &&
                 x.Deleted == false
            ,
            filterForm.PageNumber, filterForm.PageSize);
        var vehicleDtos = _mapper.Map<List<VehiclesDto>>(vehicles);
        vehicleDtos.ForEach(x => x = _dtoTranslationService.TranslateEnums(x));
        return (vehicleDtos, totalCount, null);
    }

    public async Task<(bool?, string? error)> DeleteVehicleAsync(Guid id)
    {
        var vehicle = await _repositoryWrapper.VehiclesRepository.Get(
            x => x.Id == id
        );
        if (vehicle == null)
        {
            return (false, _localize.GetLocalizedString("VehicleNotFound"));
        }

        vehicle.Deleted = true;

        await _repositoryWrapper.VehiclesRepository.Update(vehicle, id);
        return (true, null);
    }

    public async Task<(bool, string? error)> FavoriteVehicleAsync(Guid id, Guid userId)
    {
        var vehicle = await _repositoryWrapper.VehiclesRepository.GetById(id);
        if (vehicle == null)
        {
            return (false, _localize.GetLocalizedString("VehicleNotFound"));
        }

        var user = await _repositoryWrapper.CustomerRepository.GetById(userId);
        if (user == null)
        {
            return (false, _localize.GetLocalizedString("UserNotFound"));
        }

        user.FavoriteVehicles.Add(vehicle);
        var result = await _repositoryWrapper.CustomerRepository.Update(user, userId);

        if (result == null)
        {
            return (false, _localize.GetLocalizedString("FailedToFavoriteVehicle"));
        }

        return (true, null);
    }
}