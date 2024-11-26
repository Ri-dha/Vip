using Microsoft.AspNetCore.Mvc;
using VipTest.Localization;
using VipTest.reviews.dtos;
using VipTest.reviews.Payloads;
using VipTest.reviews.utli;
using VipTest.Utlity;
using VipTest.Utlity.Basic;

namespace VipTest.reviews;

[Route("/reviews")]
public class ReviewController:BaseController
{
    private readonly IReviewService _reviewService;
    private readonly IEnumTranslationService _enumTranslationService;

    public ReviewController(IReviewService reviewService, IEnumTranslationService enumTranslationService)
    {
        _reviewService = reviewService;
        _enumTranslationService = enumTranslationService;
    }
    
    [HttpGet("get-all-reviews")]
    public async Task<ActionResult<Page<ReviewDto>>> GetAll([FromQuery] ReviewFilterForm filterForm)
    {
        var (reviewDtos, totalCount, error) = await _reviewService.GetAllReviewsAsync(filterForm);
        if (error != null) return BadRequest(new { error });
        return Ok(new { data = reviewDtos, totalCount, filterForm.PageNumber, filterForm.PageSize });
    }
    
    [HttpGet("get-review/{id}")]
    public async Task<IActionResult> GetById( Guid id)
    {
        var result = await _reviewService.GetReviewByIdAsync(id);
        return Ok(result);
    }
    
    [HttpPost("create-review")]
    public async Task<IActionResult> Create([FromBody] ReviewCreateForm form)
    {
        var result = await _reviewService.CreateReviewsAsync(form);
        return Ok(result);
    }   
    
    [HttpGet("get-all-reviews-for")]
    public IActionResult GetReviewForTranslations()
    {
        var reviewFor = _enumTranslationService.GetAllEnumTranslations<ReviewFor>();
        var response = new { data =reviewFor };
        return Ok(response);
    }
    
    
    
}