using Microsoft.AspNetCore.Mvc;
using VipTest.Localization;
using VipTest.RideBillings.Models;
using VipTest.RideBillings.Payloads;
using VipTest.Rides.Utli;
using VipTest.Utlity.Basic;

namespace VipTest.RideBillings;
[Route("RideBillingTypesConfig")]
public class RideBillingTypesConfigController:BaseController
{
    
    private readonly IRideBillingTypesConfigService _rideBillingTypesConfigService;
    private readonly IEnumTranslationService _enumTranslationService;

    public RideBillingTypesConfigController(IRideBillingTypesConfigService rideBillingTypesConfigService, IEnumTranslationService enumTranslationService)
    {
        _rideBillingTypesConfigService = rideBillingTypesConfigService;
        _enumTranslationService = enumTranslationService;
    }

    
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] RideBillingTypesConfigCreateForm typesConfigCreateForm)
    {
        var (rideBillingTypesConfigDto, error) = await _rideBillingTypesConfigService.CreateAsync(typesConfigCreateForm);
        if (error != null) return BadRequest(new {error});
        return Ok(rideBillingTypesConfigDto);
    }
    
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] RideBillingTypesConfigUpdateForm typesConfigUpdateForm)
    {
        var (rideBillingTypesConfigDto, error) = await _rideBillingTypesConfigService.UpdateAsync(id, typesConfigUpdateForm);
        if (error != null) return BadRequest(new {error});
        return Ok(rideBillingTypesConfigDto);
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var (rideBillingTypesConfigDto, error) = await _rideBillingTypesConfigService.DeleteAsync(id);
        if (error != null) return BadRequest(new {error});
        return Ok(rideBillingTypesConfigDto);
    }
    
    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var (rideBillingTypesConfigDto, error) = await _rideBillingTypesConfigService.GetByIdAsync(id);
        if (error != null) return BadRequest(new {error});
        return Ok(rideBillingTypesConfigDto);
    }
    
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll([FromQuery] RideBillingTypesConfigFilterForm filter)
    {
        var (rideBillingTypesConfigDtos,totalCount, error) = await _rideBillingTypesConfigService.GetAllAsync(filter);
        if (error != null) return BadRequest(new {error});
        return Ok(rideBillingTypesConfigDtos);
    }
    
    [HttpGet("get-by-ride-type/{rideType}")]
    public async Task<IActionResult> GetByRideType(RideType rideType)
    {
        var (rideBillingTypesConfigDtos, error) = await _rideBillingTypesConfigService.GetByRideTypeAsync(rideType);
        if (error != null) return BadRequest(new {error});
        return Ok(rideBillingTypesConfigDtos);
    }
    
    
    
    [HttpGet("get-ride-types")]
    public IActionResult GetRideTypeTranslations()
    {
        var rideTypes = _enumTranslationService.GetAllEnumTranslations<RideType>();
        var response = new {data = rideTypes};
        return Ok(response);
    }

}