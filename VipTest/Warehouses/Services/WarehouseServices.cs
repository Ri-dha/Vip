using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VipProjectV0._1.Db;
using VipTest.attachmentsConfig;
using VipTest.Localization;
using VipTest.Users.BranchManagers;
using VipTest.Users.Models;
using VipTest.Wallets.Services;
using VipTest.Warehouses.Dto;
using VipTest.Warehouses.Models;
using VipTest.Warehouses.Payloads;

namespace VipTest.Warehouses.Services;

public interface IWarehouseServices
{
    Task<(WarehouseDto? warehouseDto, string? error)> CreateWarehouseAsync(WarehouseCreateForm form);
    Task<(WarehouseDto? warehouseDto, string? error)> UpdateWarehouseAsync(Guid id, WarehouseUpdateForm form);
    Task<(WarehouseDto? warehouseDto, string? error)> GetWarehouseByIdAsync(Guid id);

    Task<(List<WarehouseDto>? warehouseDtos, int? totalCount, string? error)> GetAllWarehousesAsync(
        WarehouseFilterForm filterForm);

    Task<(WarehouseDto? warehouseDto, string? error)> DeleteWarehouseAsync(Guid id);
}

public class WarehouseServices : IWarehouseServices
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localize;
    private readonly IDtoTranslationService _dtoTranslationService;
    private readonly IWalletService _walletService;


    public WarehouseServices(IRepositoryWrapper repositoryWrapper, IMapper mapper,
        ILocalizationService localize, IDtoTranslationService dtoTranslationService, IWalletService walletService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _localize = localize;
        _dtoTranslationService = dtoTranslationService;
        _walletService = walletService;
    }


    
    public async Task<(WarehouseDto? warehouseDto, string? error)> CreateWarehouseAsync(WarehouseCreateForm form)
    {
        var warehouseExists =
            await _repositoryWrapper.WarehouseRepository.Get(x => x.WarehousePhone == form.WarehousePhone);
        if (warehouseExists != null)
        {
            return (null, _localize.GetLocalizedString("UserAlreadyExists"));
        }
        
        var branchManagerExists =
            await _repositoryWrapper.BranchManagerRepository.Get(x => x.Username == form.WarehousePhone);
        if (branchManagerExists != null)
        {
            return (null, _localize.GetLocalizedString("UserAlreadyExists"));
        }
        

        // Create and map Warehouse (without assigning BranchManagerId yet)
        var warehouse = _mapper.Map<Warehouse>(form);

        if (form.Attachments != null && form.Attachments.Count > 0)
        {
            warehouse.Attachments = _mapper.Map<List<Attachments>>(form.Attachments);
        }
        
        
        // Create a new BranchManager
        var branchManager = new BranchManager
        {
            Username = form.WarehouseName,
            PhoneNumber = form.WarehousePhone,
            Password = BCrypt.Net.BCrypt.HashPassword(form.Password),
            Role = Roles.BranchManager,
            isActive = true
        };


        // Set profile images if provided
        if (form.ProfileImage != null)
        {
            branchManager.ProfileImage = form.ProfileImage;
            warehouse.ProfileImage = form.ProfileImage;
        }

        // Save BranchManager first
        var branchManagerResult = await _repositoryWrapper.BranchManagerRepository.Add(branchManager);
        if (branchManagerResult == null)
        {
            var error = _localize.GetLocalizedString("FailedToCreateBranchManager");
            return (null, error);
        }

        // Now that the BranchManager is saved, assign the BranchManagerId to the warehouse
        warehouse.BranchManagerId = branchManagerResult.Id;
        warehouse.BranchManager = branchManagerResult;

        

        // Save Warehouse now
        var result = await _repositoryWrapper.WarehouseRepository.Add(warehouse);
        if (result == null)
        {
            var error = _localize.GetLocalizedString("FailedToCreateWarehouse");
            return (null, error);
        }

        var warehouseId = result.Id;

        var branchManagerUpdateresult =
            await _repositoryWrapper.BranchManagerRepository.Get(x => x.Id == branchManagerResult.Id);

        branchManagerUpdateresult.WarehouseId = warehouseId;
        branchManagerUpdateresult.Warehouse = result;

        await _repositoryWrapper.BranchManagerRepository.Update(branchManagerUpdateresult, branchManagerResult.Id);

        return (_mapper.Map<WarehouseDto>(result), null);
    }


    public async Task<(WarehouseDto? warehouseDto, string? error)> UpdateWarehouseAsync(Guid id,
        WarehouseUpdateForm form)
    {
        var warehouse = await _repositoryWrapper.WarehouseRepository.Get(
            x => x.Id == id,
            include: source => source.Include(x => x.BranchManager)
                .Include(x => x.WarehouseVehicles)
                .Include(x => x.Attachments)
        );
        if (warehouse == null)
        {
            return (null, _localize.GetLocalizedString("WarehouseNotFound"));
        }

        var branchManager = await _repositoryWrapper.BranchManagerRepository.Get(x => x.Id == warehouse.BranchManagerId,
            include: source => source.Include(x => x.Warehouse)
        );
        if (branchManager == null)
        {
            return (null, _localize.GetLocalizedString("BranchManagerNotFound"));
        }

        bool isUserChanged = false;

        if (!string.IsNullOrEmpty(form.Password))
        {
            branchManager.Password = BCrypt.Net.BCrypt.HashPassword(form.Password);
            isUserChanged = true;
        }

        if (!string.IsNullOrEmpty(form.WarehouseName))
        {
            branchManager.Username = form.WarehouseName;
            isUserChanged = true;
        }

        if (!string.IsNullOrEmpty(form.WarehousePhone))
        {
            branchManager.PhoneNumber = form.WarehousePhone;
            isUserChanged = true;
        }
        
        if (form.ProfileImage != null)
        {
            branchManager.ProfileImage = form.ProfileImage;
            warehouse.ProfileImage = form.ProfileImage;
            isUserChanged = true;
        }

        if (form.IsActive != null)
        {
            branchManager.isActive = form.IsActive.Value;
            isUserChanged = true;
        }

        if (isUserChanged)
        {
            await _repositoryWrapper.BranchManagerRepository.Update(branchManager, branchManager.Id);
        }
        
        if (form.Attachments != null && form.Attachments.Count > 0)
        {
            warehouse.Attachments.RemoveAll(x => x.Id != null);
            warehouse.Attachments = _mapper.Map<List<Attachments>>(form.Attachments);
        }
        
        if (form.OperationPrecantage != null)
        {
            warehouse.OperationPrecantage = form.OperationPrecantage.Value;
        }

        _mapper.Map(form, warehouse);
        var result = await _repositoryWrapper.WarehouseRepository.Update(warehouse, id);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToUpdateWarehouse"));
        }

        return (_mapper.Map<WarehouseDto>(result), null);
    }

    public async Task<(WarehouseDto? warehouseDto, string? error)> GetWarehouseByIdAsync(Guid id)
    {
        var result = await _repositoryWrapper.WarehouseRepository.Get( x => x.Id == id,
            include: source => source.Include(x => x.BranchManager)
                .Include(x => x.WarehouseVehicles)
                .Include(x => x.Attachments)
        );
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("WarehouseNotFound"));
        }

        var warehouseDto = _mapper.Map<WarehouseDto>(result);
        warehouseDto = _dtoTranslationService.TranslateEnums(warehouseDto);
        return (warehouseDto, null);
    }

    public async Task<(List<WarehouseDto>? warehouseDtos, int? totalCount, string? error)> GetAllWarehousesAsync(
        WarehouseFilterForm filterForm)
    {
        var (warehouses, totalCount) =
            await _repositoryWrapper.WarehouseRepository.GetAll(
                x =>
                    (string.IsNullOrEmpty(filterForm.WarehouseName) ||
                     x.WarehouseName.Contains(filterForm.WarehouseName)) &&
                    (string.IsNullOrEmpty(filterForm.WarehouseLocation) ||
                     x.WarehouseLocation.Contains(filterForm.WarehouseLocation)) &&
                    (filterForm.Governorate == null || x.Governorate == filterForm.Governorate) &&
                    (filterForm.IsActive == null || x.BranchManager.isActive == filterForm.IsActive)&&
                x.Deleted == false
                ,
                include: src => src.Include(x => x.BranchManager)
                    .Include(x => x.WarehouseVehicles)
                , filterForm.PageNumber, filterForm.PageSize);

        var warehouseDtos = _mapper.Map<List<WarehouseDto>>(warehouses);
        warehouseDtos.ForEach(x => _dtoTranslationService.TranslateEnums(x));
        return (warehouseDtos, totalCount, null);
    }


    public async Task<(WarehouseDto? warehouseDto, string? error)> DeleteWarehouseAsync(Guid id)
    {
        var warehouse = await _repositoryWrapper.WarehouseRepository.Get(x => x.Id == id,
            include: source => source.Include(x => x.WarehouseVehicles)!
                .Include(x => x.BranchManager)!
        );
        if (warehouse == null)
        {
            return (null, _localize.GetLocalizedString("WarehouseNotFound"));
        }

        warehouse.Deleted = true;
        
        var branchManager = await _repositoryWrapper.BranchManagerRepository.Get(x => x.Id == warehouse.BranchManagerId);
        if (branchManager == null)
        {
            return (null, _localize.GetLocalizedString("BranchManagerNotFound"));
        }
        
        branchManager.isActive = false;
        await _repositoryWrapper.BranchManagerRepository.Update(branchManager, branchManager.Id);

        var warehouseVehicles = warehouse.WarehouseVehicles;
        if (warehouseVehicles != null && warehouseVehicles.Count > 0)
        {
            foreach (var vehicle in warehouseVehicles)
            {
                vehicle.Deleted = true;
                await _repositoryWrapper.VehiclesRepository.Update(vehicle, vehicle.Id);
            }
        }
        
        await _repositoryWrapper.WarehouseRepository.Update(warehouse, id);

        return (_mapper.Map<WarehouseDto>(warehouse), null);
    }
}
