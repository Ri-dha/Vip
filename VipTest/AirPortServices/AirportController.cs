using Microsoft.AspNetCore.Mvc;
using VipTest.AirPortServices.Dto;
using VipTest.AirPortServices.Payloads;
using VipTest.AirPortServices.Services;
using VipTest.AirPortServices.Utli;
using VipTest.Localization;
using VipTest.Utlity;
using VipTest.Utlity.Basic;

namespace VipTest.AirPortServices;


[Route("/airport-services")]
public class AirportController:BaseController
{
    
    private readonly IAirportServices _services;
    private readonly IEnumTranslationService _enumTranslationService;

    public AirportController(IEnumTranslationService enumTranslationService, IAirportServices services)
    {
        _enumTranslationService = enumTranslationService;
        _services = services;
    }


    [HttpPost("create-visa-service")]
    public async Task<IActionResult> Create([FromBody] VisaVipServiceCreateForm form)
    {
        var (success, error) = await _services.CreateVisaService(form);
        if (error != null) return BadRequest(new { error });
        return Ok(success);
    }

    [HttpPost("create-lounge-service")]
    public async Task<IActionResult> Create([FromBody] LoungeServiceCreateForm form)
    {
        var (success, error) = await _services.CreateLoungeService(form);
        if (error != null) return BadRequest(new { error });
        return Ok(success);
    }

    [HttpPost("create-missing-luggage-service")]
    public async Task<IActionResult> Create([FromBody] LuggageServiceCreateForm form)
    {
        var (success, error) = await _services.CreateLuggageService(form);
        if (error != null) return BadRequest(new { error });
        return Ok(success);
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var (service, error) = await _services.Get(id);
        if (error != null) return BadRequest(new { error });
        return Ok(service);
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll([FromQuery] AirportServicesFilterForm form)
    {
        var (services, total, error) = await _services.GetAll(form);
        if (error != null) return BadRequest(new { error });

        var result = new Page<AirPortServicesDto>
        {
            Data = services,
            PagesCount = (int)Math.Ceiling((double)(total ?? 0) / form.PageSize),
            CurrentPage = form.PageNumber,
            TotalCount = total
        };

        return Ok(result);
    }


    [HttpPut("update-visa-service/{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] VisaVipUpdateForm form)
    {
        var (service, error) = await _services.UpdateVisaService(id, form);
        if (error != null) return BadRequest(new { error });
        return Ok(service);
    }
    
    

    [HttpPut("update-lounge-service/{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] LoungeUpdateForm form)
    {
        var (service, error) = await _services.UpdateLoungeService(id, form);
        if (error != null) return BadRequest(new { error });
        return Ok(service);
    }

    [HttpPut("update-luggage-service/{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] LuggageUpdateForm form)
    {
        var (service, error) = await _services.UpdateLuggageService(id, form);
        if (error != null) return BadRequest(new { error });
        return Ok(service);
    }

    [HttpPut("change-status/{id}")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromQuery] AirportServicesStatus status, [FromBody] string? note)
    {
        var (service, error) = await _services.UpdateStatus(id, status, note);
        if (error != null) return BadRequest(new { error });
        return Ok(service);
    }
    
    [HttpPut("accept-service/{id}")]
    public async Task<IActionResult> StartService(Guid id)
    {
        var (service, error) = await _services.Start(id);
        if (error != null) return BadRequest(new { error });
        return Ok(service);
    }
    
    [HttpPut("complete-service/{id}")]
    public async Task<IActionResult> FinishService(Guid id)
    {
        var (service, error) = await _services.Complete(id);
        if (error != null) return BadRequest(new { error });
        return Ok(service);
    }
    
    [HttpPut("cancel-service/{id}")]
    public async Task<IActionResult> CancelService(Guid id, [FromBody] string? note)
    {
        var (service, error) = await _services.Cancel(id, note);
        if (error != null) return BadRequest(new { error });
        return Ok(service);
    }
    
    [HttpPut("reject-service/{id}")]
    
    public async Task<IActionResult> RejectService(Guid id, [FromBody] string? note)
    {
        var (service, error) = await _services.Reject(id,note);
        if (error != null) return BadRequest(new { error });
        return Ok(service);
    }
    
    

    
    [HttpGet("get-status")]
    public IActionResult GetStatus()
    {
        var status = _enumTranslationService.GetAllEnumTranslations<AirportServicesStatus>();
        return Ok(status);
    }

    [HttpGet("get-types")]
    public IActionResult GetTypes()
    {
        var types = _enumTranslationService.GetAllEnumTranslations<AirportServicesTypes>();
        return Ok(types);
    }
    
    
}