using Microsoft.AspNetCore.Mvc;
using VipTest.Discounts.Payloads;
using VipTest.Discounts.utli;
using VipTest.Localization;
using VipTest.Utlity.Basic;

namespace VipTest.Discounts;

[Route("/discounts")]
public class DiscountController : BaseController
{
    private readonly IDiscountService _discountService;
    private readonly IEnumTranslationService _enumTranslationService;

    public DiscountController(IDiscountService discountService, IEnumTranslationService enumTranslationService)
    {
        _discountService = discountService;
        _enumTranslationService = enumTranslationService;
    }

    [HttpPost("create-discount")]
    public async Task<IActionResult> CreateDiscount([FromBody] DiscountCreateForm createForm)
    {
        var (discountDto, error) = await _discountService.CreateDiscountAsync(createForm);
        if (error != null) return BadRequest(new { error });
        return Ok(discountDto);
    }

    [HttpPut("update-discount/{id}")]
    public async Task<IActionResult> UpdateDiscount(Guid id, [FromBody] DiscountUpdateForm updateForm)
    {
        var (discountDto, error) = await _discountService.UpdateDiscountAsync(id, updateForm);
        if (error != null) return BadRequest(new { error });
        return Ok(discountDto);
    }

    [HttpDelete("delete-discount/{id}")]
    public async Task<IActionResult> DeleteDiscount(Guid id)
    {
        var (discountDto, error) = await _discountService.DeleteDiscountAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(discountDto);
    }

    [HttpGet("get-discount/{id}")]
    public async Task<IActionResult> GetDiscount(Guid id)
    {
        var (discountDto, error) = await _discountService.GetDiscountAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(discountDto);
    }

    [HttpPost("check-discount")]
    public async Task<IActionResult> CheckDiscount([FromBody] DiscountCheckForm checkForm)
    {
        var (discountDto, error) = await _discountService.CheckDiscountAsync(checkForm.DiscountCode, checkForm.UserId,
            DateTime.Now, checkForm.RideType);
        if (error != null) return BadRequest(new { error });
        return Ok(discountDto);
    }

    [HttpGet("get-all-discounts")]
    public async Task<IActionResult> GetAllDiscounts([FromQuery] DiscountFilterForm filterForm)
    {
        var (discountDtos, total, error) = await _discountService.GetDiscountsAsync(filterForm);
        if (error != null) return BadRequest(new { error });
        return Ok(new { data = discountDtos, total });
    }

    [HttpPost("add-customers-to-discount/{discountId}")]
    public async Task<IActionResult> AddCustomersToDiscount(Guid discountId, [FromBody] List<Guid> customerIds)
    {
        var (success, error) = await _discountService.AddCustomersToDiscountAsync(discountId, customerIds);
        if (error != null) return BadRequest(new { error });
        return Ok(success);
    }

    [HttpPost("remove-customers-from-discount/{discountId}")]
    public async Task<IActionResult> RemoveCustomersFromDiscount(Guid discountId, [FromBody] List<Guid> customerIds)
    {
        var (success, error) = await _discountService.RemoveCustomersFromDiscountAsync(discountId, customerIds);
        if (error != null) return BadRequest(new { error });
        return Ok(success);
    }

    [HttpGet("get-discount-services")]
    public async Task<IActionResult> GetDiscountServices()
    {
        var discountServices = _enumTranslationService.GetAllEnumTranslations<DiscountServices>();

        var response = new { data = discountServices };
        return Ok(response);
    }
}