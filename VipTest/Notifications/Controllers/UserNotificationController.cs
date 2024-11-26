using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VipTest.Localization;
using VipTest.Notifications.Dtos;
using VipTest.Notifications.PayLoads;
using VipTest.Utlity;
using VipTest.Utlity.Basic;

namespace VipTest.Notifications.Controllers;

[Route("user-notifications")]
public class UserNotificationController : BaseController
{
    private readonly IUserNotificationServices _userNotificationServices;
    private readonly IEnumTranslationService _enumTranslationService;

    public UserNotificationController(IUserNotificationServices userNotificationServices,
        IEnumTranslationService enumTranslationService)
    {
        _userNotificationServices = userNotificationServices;
        _enumTranslationService = enumTranslationService;
    }
    
    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var (userNotification, error) = await _userNotificationServices.GetById(id);
        if (error != null) return BadRequest(new { error });
        return Ok(userNotification);
    }
    
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll([FromQuery] UserNotificationFilter filter)
    {
        var (userNotifications, totalCount, error) = await _userNotificationServices.GetAll(filter);
        if (error != null) return BadRequest(new { error });

        var response = new Page<UserNotificationDto>()
        {
            Data = userNotifications,
            PagesCount = (int)Math.Ceiling((double)(totalCount ?? 0) / filter.PageSize),
            CurrentPage = filter.PageNumber,
            TotalCount = totalCount
        };
        
        return Ok(response);
        
    }
    
    [HttpPost("create")]
        [SwaggerOperation
        (
            Summary = "Creates a new user notification. This is only for testing",
            Description = "This is only for testing"
        )]
    public async Task<IActionResult> Create([FromBody] UserNotificationForm form)
    {
        var (userNotification, error) = await _userNotificationServices.Create(form);
        if (error != null) return BadRequest(new { error });
        return Ok(userNotification);
    }
    
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UserNotificationUpdateForm  form)
    {
        var (userNotification, error) = await _userNotificationServices.Update(id, form.IsRead);
        if (error != null) return BadRequest(new { error });
        return Ok(userNotification);
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var (userNotification, error) = await _userNotificationServices.Delete(id);
        if (error != null) return BadRequest(new { error });
        return Ok(userNotification);
    }
    
}