using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VipTest.Localization;
using VipTest.Rentals.Dto;
using VipTest.Rentals.PayLoads;
using VipTest.Rentals.utli;
using VipTest.Utlity;
using VipTest.Utlity.Basic;

namespace VipTest.Rentals;

[Route("/car-rentals")]
public class CarRentalController : BaseController
{
    private readonly ICarRentalOrderService _carRentalService;
    private readonly IEnumTranslationService _enumTranslationService;

    public CarRentalController(ICarRentalOrderService carRentalService, IEnumTranslationService enumTranslationService)
    {
        _carRentalService = carRentalService;
        _enumTranslationService = enumTranslationService;
    }


    [HttpPost("create")]
    [SwaggerOperation(Summary = "Creates a new car rental order.",
        Description = "This endpoint allows creating a new car rental order. " +
                      "Note: Some fields are nullable, as indicated in the schema.")]
    public async Task<IActionResult> CreateCarRentalOrder([FromBody] CarRentalOrderCreateForm carRentalOrder)
    {
        var result = await _carRentalService.CreateAsync(carRentalOrder);
        return Ok(result);
    }

    [HttpPut("update/{id}")]
    [SwaggerOperation(Summary = "Updates an existing car rental order.",
        Description = "This endpoint allows updating an existing car rental order. " +
                      "Note: Some fields are nullable, as indicated in the schema.")]
    public async Task<IActionResult> UpdateCarRentalOrder(Guid id, [FromBody] CarRentalOrderUpdateForm carRentalOrder)
    {
        var result = await _carRentalService.UpdateAsync(id, carRentalOrder);
        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteCarRentalOrder(Guid id)
    {
        var result = await _carRentalService.DeleteAsync(id);
        return Ok(result);
    }
    
    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetCarRentalOrder(Guid id)
    {
        var result = await _carRentalService.GetByIdAsync(id);
        return Ok(result);
    }
    
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllCarRentalOrders([FromQuery] CarRentalOrderFilterForm filterForm)
    {
        var (carRentalOrders, totalCount, error) = await _carRentalService.GetListAsync(filterForm);
        if (error != null) return BadRequest(new { error });
        var result = new Page<CarRentalOrderDto>()
        {
            Data = carRentalOrders,
            PagesCount = (int)Math.Ceiling((double)(totalCount ?? 0) / filterForm.PageSize),
            CurrentPage = filterForm.PageNumber,
            TotalCount = totalCount
        };
        
        return Ok(result);
    }
    
    [HttpPut("update-status/{id}")]
    public async Task<IActionResult> UpdateCarRentalOrderStatus(Guid id, [FromQuery] RentalOrderStatus status,string? note)
    {
        var (carRentalOrder, error) = await _carRentalService.ChangeStatusAsync(id, status, note);
        if (error != null) return BadRequest(new { error });
        return Ok(carRentalOrder);
    }
    
    [HttpPut("confirm/{id}")]
    public async Task<IActionResult> ConfirmCarRentalOrder(Guid id)
    {
        var (carRentalOrder, error) = await _carRentalService.ConfirmAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(carRentalOrder);
    }
    
    
    [HttpPut("reject/{id}")]
    public async Task<IActionResult> RejectCarRentalOrder(Guid id, string? note)
    {
        var (carRentalOrder, error) = await _carRentalService.RejectAsync(id, note);
        if (error != null) return BadRequest(new { error });
        return Ok(carRentalOrder);
    }
    
    [HttpPut("cancel/{id}")]   
    public async Task<IActionResult> CancelCarRentalOrder(Guid id, string? note)
    {
        var (carRentalOrder, error) = await _carRentalService.CancelAsync(id, note);
        if (error != null) return BadRequest(new { error });
        return Ok(carRentalOrder);
    }
    
    [HttpPut("pick-up/{id}")]
    public async Task<IActionResult> PickUpCarRentalOrder(Guid id)
    {
        var (carRentalOrder, error) = await _carRentalService.PickedUpAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(carRentalOrder);
    }
    
    //
    // [HttpPut("drop-off/{id}")]   
    // public async Task<IActionResult> DropOffCarRentalOrder(Guid id)
    // {
    //     var (carRentalOrder, error) = await _carRentalService.DropOffCodeAsync(id);
    //     if (error != null) return BadRequest(new { error });
    //     return Ok(carRentalOrder);
    // }
    //
    //
    // [HttpPut("car-back/{id}")]
    // public async Task<IActionResult> CarBackCarRentalOrder(Guid id)
    // {
    //     var (carRentalOrder, error) = await _carRentalService.CarBackAsync(id);
    //     if (error != null) return BadRequest(new { error });
    //     return Ok(carRentalOrder);
    // }
    //     
    
    [HttpPut("complete/{id}")]
    public async Task<IActionResult> CompleteCarRentalOrder(Guid id)
    {
        var (carRentalOrder, error) = await _carRentalService.FinishAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(carRentalOrder);
    }
    
    
    [HttpPut("assigne-driver/{id}")]
    public async Task<IActionResult> AssignDriverToCarRentalOrder(Guid id,[FromBody] Guid driverId)
    {
        var (carRentalOrder, error) = await _carRentalService.AssignDriverAsync(id, driverId);
        if (error != null) return BadRequest(new { error });
        return Ok(carRentalOrder);
    }
    
        
        
        
    [HttpGet("get-pick-up-types")]
    public IActionResult GetRideTypeTranslations()
    {
        var pickUpTypes = _enumTranslationService.GetAllEnumTranslations<PickUpType>();
        var response = new { data = pickUpTypes };
        return Ok(response);
    }

    [HttpGet("get-rental-order-status")]
    public IActionResult GetRideDestinationTranslations()
    {
        var rentalOrderStatuses = _enumTranslationService.GetAllEnumTranslations<RentalOrderStatus>();
        var response = new { data = rentalOrderStatuses };
        return Ok(response);
    }
    
    
}