using Microsoft.AspNetCore.Mvc;
using VipTest.Localization;
using VipTest.Utlity;
using VipTest.Utlity.Basic;
using VipTest.vehicles.Dtos;
using VipTest.vehicles.PayLoads;
using VipTest.vehicles.Services;
using VipTest.vehicles.Utli;

namespace VipTest.vehicles.Controller;


[Route("rental-cars/")]
public class RentCarsController:BaseController
{
    private readonly IRentCarsServices _vehicleServices;
    private readonly IEnumTranslationService _enumTranslationService;

    public RentCarsController(IRentCarsServices vehicleServices, IEnumTranslationService enumTranslationService)
    {
        _vehicleServices = vehicleServices;
        _enumTranslationService = enumTranslationService;
    }
    
    
     [HttpPost("create-vehicle")]
    public async Task<IActionResult> CreateVehicle([FromBody] RentCarsCreateForm vehicleCreateForm)
    {
        var (vehicleDto, error) = await _vehicleServices.CreateRentCarAsync(vehicleCreateForm);
        if (error != null) return BadRequest(new { error });
        return Ok(vehicleDto);
    }
    
    [HttpPut("update-vehicle/{id}")]
    public async Task<IActionResult> UpdateVehicle(Guid id, [FromBody] RentCarsUpdateForm vehicleUpdateForm)
    {
        var (vehicleDto, error) = await _vehicleServices.UpdateRentCarAsync(id, vehicleUpdateForm);
        if (error != null) return BadRequest(new { error });
        return Ok(vehicleDto);
    }
    
    [HttpGet("get-vehicle/{id}")]
    public async Task<IActionResult> GetVehicleById(Guid id)
    {
        var (vehicleDto, error) = await _vehicleServices.GetRentCarByIdAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(vehicleDto);
    }
    
    [HttpGet("get-all-vehicles")]
    public async Task<IActionResult> GetAllVehicles([FromQuery] VehicleFilterForm filterForm)
    {
        var (vehicleDtos, totalCount, error) = await _vehicleServices.GetAllRentCarsAsync(filterForm);
        if (error != null) return BadRequest(new { error });
        
        var result = new Page<RentCarsDto>()
        {
            Data = vehicleDtos,
            PagesCount = (int)Math.Ceiling((double)(totalCount ?? 0) / filterForm.PageSize),
            CurrentPage = filterForm.PageNumber,
            TotalCount = totalCount
        };
        
        return Ok(result);
    }
    
    [HttpDelete("delete-vehicle/{id}")]
    public async Task<IActionResult> DeleteVehicle(Guid id)
    {
        var (vehicleDto, error) = await _vehicleServices.DeleteRentCarAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(vehicleDto);
    }
    
    [HttpPost("favorite-vehicle/{id}")]
    public async Task<IActionResult> FavoriteVehicle(Guid id, [FromQuery] Guid userId)
    {
        var (isFavorited, error) = await _vehicleServices.FavoriteRentCarAsync(id, userId);
        if (error != null) return BadRequest(new { error });
        return Ok(new { isFavorited });
    }
    
    [HttpGet("get-favorite-vehicles")]
    public async Task<IActionResult> GetFavoriteVehicles([FromQuery] Guid userId)
    {
        var (vehicleDtos, error) = await _vehicleServices.GetFavoriteRentCarsAsync(userId);
        if (error != null) return BadRequest(new { error });
        return Ok(vehicleDtos);
    }
    
    [HttpGet("get-vehicle-statuses")]
    public IActionResult GetVehicleStatusTranslation()
    {
        var vehicleStatuses = _enumTranslationService.GetAllEnumTranslations<VehicleStatus>();
        return Ok(vehicleStatuses);
    }
    
    [HttpGet("get-vehicle-types")]
    public IActionResult GetVehicleTypeTranslation()
    {
        var vehicleTypes = _enumTranslationService.GetAllEnumTranslations<VehicleType>();
        var response = new { data = vehicleTypes };
        return Ok(response);
    }
    
    [HttpGet("get-car-types")]
    public IActionResult GetCarTypeTranslation()
    {
        var carTypes = _enumTranslationService.GetAllEnumTranslations<CarType>();
        var response = new { data = carTypes };
        return Ok(response);
    }
    
    [HttpGet("get-shifter-types")]
    public IActionResult GetShifterTypeTranslation()
    {
        var shifterTypes = _enumTranslationService.GetAllEnumTranslations<ShifterType>();
        var response = new { data = shifterTypes };
        return Ok(response);
    }
    
    [HttpGet("get-car-brands")]
    public IActionResult GetVehicleBrandTranslation()
    {
        var vehicleBrands = _enumTranslationService.GetAllEnumTranslations<CarBrand>();
        var response = new { data = vehicleBrands };
        return Ok(response);
    }
    
    [HttpGet("get-car-acceptence-statuses")]
    public IActionResult GetVehicleAcceptenceStatusTranslation()
    {
        var vehicleAcceptenceStatuses = _enumTranslationService.GetAllEnumTranslations<CarAcceptanceStatus>();
        return Ok(vehicleAcceptenceStatuses);
    }
}