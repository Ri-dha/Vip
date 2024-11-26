using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Swashbuckle.AspNetCore.Annotations;
using VipTest.Localization;
using VipTest.Rides.Dto;
using VipTest.Rides.Payloads;
using VipTest.Rides.Utli;
using VipTest.Utlity;
using VipTest.Utlity.Basic;

namespace VipTest.Rides;

[Route("/Rides")]
public class RideController : BaseController
{
    private readonly IRideService _rideService;
    private readonly IEnumTranslationService _enumTranslationService;


    public RideController(IRideService rideService, IEnumTranslationService enumTranslationService)
    {
        _rideService = rideService;
        _enumTranslationService = enumTranslationService;
    }

    [HttpPost("create")]
    [SwaggerOperation(
        Summary = "Creates a new ride.",
        Description = "This endpoint allows creating a new ride. " +
                      "Note: Some fields are nullable, as indicated in the schema."
    )]
    public async Task<IActionResult> Create([FromBody] RideCreateForm rideCreateForm)
    {
        var (rideDto, error) = await _rideService.CreateRideAsync(rideCreateForm);
        if (error != null) return BadRequest(new { error });
        return Ok(rideDto);
    }

    [HttpPut("update/{id}")]
    [SwaggerOperation(
        Summary = "Updates an existing ride.",
        Description = "This endpoint allows updating an existing ride. " +
                      "Note: Some fields are nullable, as indicated in the schema."
    )]
    public async Task<IActionResult> Update(Guid id, [FromBody] RideUpdateForm rideUpdateForm)
    {
        var (rideDto, error) = await _rideService.UpdateRideAsync(id, rideUpdateForm);
        if (error != null) return BadRequest(new { error });
        return Ok(rideDto);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var (rideDto, error) = await _rideService.DeleteRideAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(rideDto);
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var (rideDto, error) = await _rideService.GetRideByIdAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(rideDto);
    }


    [HttpGet("get-all-rides")]
    public async Task<ActionResult<Page<RideDto>>> GetAllRides([FromQuery] RideFilterForm filter)
    {
        var (rides, totalCount, error) = await _rideService.GetAllRidesAsync(filter);

        if (error != null)
        {
            return BadRequest(new { error });
        }

        var result = new Page<RideDto>()
        {
            Data = rides,
            PagesCount = (int)Math.Ceiling((double)(totalCount ?? 0) / filter.PageSize),
            CurrentPage = filter.PageNumber,
            TotalCount = totalCount
        };

        return Ok(result);
    }


    [HttpPut("change-status/{id}")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromQuery] RideStatus status, [FromBody] string? note)
    {
        var (rideDto, error) = await _rideService.ChangeRideStatus(id, status, note);
        if (error != null) return BadRequest(new { error });
        return Ok(rideDto);
    }

    [HttpPut("confirm/{id}")]
    public async Task<IActionResult> ConfirmRide(Guid id)
    {
        var (rideDto, error) = await _rideService.ConfirmRideAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(rideDto);
    }

    [HttpPut("reject/{id}")]
    public async Task<IActionResult> RejectRide(Guid id, [FromBody] string? rejectionReason)
    {
        var (rideDto, error) = await _rideService.RejectRideAsync(id, rejectionReason);
        if (error != null) return BadRequest(new { error });
        return Ok(rideDto);
    }

    [HttpPut("cancel/{id}")]
    public async Task<IActionResult> CancelRide(Guid id, [FromBody] string? cancelationReason)
    {
        var (rideDto, error) = await _rideService.CancelRideAsync(id, cancelationReason);
        if (error != null) return BadRequest(new { error });
        return Ok(rideDto);
    }

    [HttpPut("start/{id}")]
    public async Task<IActionResult> StartRide(Guid id)
    {
        var (rideDto, error) = await _rideService.StartRideAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(rideDto);
    }

    [HttpPut("arrived-at-pickup/{id}")]
    public async Task<IActionResult> ArrivedAtPickup(Guid id)
    {
        var (rideDto, error) = await _rideService.ArrivedAtPickupAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(rideDto);
    }

    [HttpPut("arrived-at-detour/{id}")]
    public async Task<IActionResult> ArrivedAtDetour(Guid id)
    {
        var (rideDto, error) = await _rideService.ArrivedAtDetourAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(rideDto);
    }

    [HttpPut("complete/{id}")]
    public async Task<IActionResult> CompleteRide(Guid id)
    {
        var (rideDto, error) = await _rideService.CompleteRideAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(rideDto);
    }


    [HttpGet("get-ride-status")]
    public IActionResult GetRideStatusTranslations()
    {
        var rideStatuses = _enumTranslationService.GetAllEnumTranslations<RideStatus>();
        var response = new { data = rideStatuses };
        return Ok(response);
    }

    [HttpGet("get-ride-payment-types")]
    public IActionResult GetRidePaymentTypeTranslations()
    {
        var ridePaymentTypes = _enumTranslationService.GetAllEnumTranslations<RidePaymentType>();
        return Ok(ridePaymentTypes);
    }


    [HttpGet("get-ride-types")]
    public IActionResult GetRideTypeTranslations()
    {
        var rideTypes = _enumTranslationService.GetAllEnumTranslations<RideType>();
        var response = new { data = rideTypes };
        return Ok(response);
    }

    [HttpGet("get-ride-destiantion")]
    public IActionResult GetRideDestinationTranslations()
    {
        var rideDestinations = _enumTranslationService.GetAllEnumTranslations<Destiantion>();
        var response = new { data = rideDestinations };
        return Ok(response);
    }
}