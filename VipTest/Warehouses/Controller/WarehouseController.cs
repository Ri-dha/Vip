using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VipTest.Localization;
using VipTest.Utlity;
using VipTest.Utlity.Basic;
using VipTest.Warehouses.Dto;
using VipTest.Warehouses.Payloads;
using VipTest.Warehouses.Services;

namespace VipTest.Warehouses.Controller;

[Route("warehouses/")]
public class WarehouseController : BaseController
{
    private readonly IWarehouseServices _warehouseServices;
    private readonly IEnumTranslationService _enumTranslationService;

    public WarehouseController(IWarehouseServices warehouseServices, IEnumTranslationService enumTranslationService)
    {
        _warehouseServices = warehouseServices;
        _enumTranslationService = enumTranslationService;
    }

    [HttpPost("create-warehouse")]
    // [Authorize(Policy = "ManagerPolicy")]
    public async Task<IActionResult> CreateWarehouse([FromBody] WarehouseCreateForm warehouseCreateForm)
    {
        var (warehouseDto, error) = await _warehouseServices.CreateWarehouseAsync(warehouseCreateForm);
        if (error != null) return BadRequest(new { error });
        return Ok(warehouseDto);
    }

    [HttpPut("update-warehouse/{id}")]
    public async Task<IActionResult> UpdateWarehouse(Guid id, [FromBody] WarehouseUpdateForm warehouseUpdateForm)
    {
        var (warehouseDto, error) = await _warehouseServices.UpdateWarehouseAsync(id, warehouseUpdateForm);
        if (error != null) return BadRequest(new { error });
        return Ok(warehouseDto);
    }

    [HttpGet("get-warehouse/{id}")]
    public async Task<IActionResult> GetWarehouseById(Guid id)
    {
        var (warehouseDto, error) = await _warehouseServices.GetWarehouseByIdAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(warehouseDto);
    }

    [HttpGet("get-all-warehouses")]
    public async Task<IActionResult> GetAllWarehouses([FromQuery] WarehouseFilterForm filterForm)
    {
        var (warehouses, totalCount, error) = await _warehouseServices.GetAllWarehousesAsync(filterForm);
        if (error != null) return BadRequest(new { error });
    
        var result = new Page<WarehouseDto>()
        {
            Data = warehouses,
            PagesCount = (int)Math.Ceiling((double)(totalCount ?? 0) / filterForm.PageSize),
            CurrentPage = filterForm.PageNumber,
            TotalCount = totalCount
        };
        return Ok(result);
    }

    [HttpDelete("delete-warehouse/{id}")]
    public async Task<IActionResult> DeleteWarehouse(Guid id)
    {
        var (warehouseDto, error) = await _warehouseServices.DeleteWarehouseAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(warehouseDto);
    }
    
    [HttpGet("get-governorates")]
    public IActionResult GetGovernoratesTranslation()
    {
        var governorates = _enumTranslationService.GetAllEnumTranslations<IraqGovernorates>();
        var response = new { data = governorates };
        return Ok(response);
    }

}