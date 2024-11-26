using Microsoft.AspNetCore.Mvc;
using VipTest.Localization;
using VipTest.Notifications.Dtos;
using VipTest.Notifications.Models;
using VipTest.Notifications.PayLoads;
using VipTest.Utlity;
using VipTest.Utlity.Basic;

namespace VipTest.Notifications.Controllers;

[Route("notification-templates")]
public class NotificationTemplateController:BaseController
{
    
    private readonly INotificationTemplateServices _notificationTemplateServices;
    private readonly IEnumTranslationService _enumTranslationService;

    public NotificationTemplateController(IEnumTranslationService enumTranslationService, INotificationTemplateServices notificationTemplateServices)
    {
        _enumTranslationService = enumTranslationService;
        _notificationTemplateServices = notificationTemplateServices;
    }
    
    [HttpPost("create-notification-template")]
    public async Task<IActionResult> CreateNotificationTemplate([FromBody] NotificationTemplateForm form)
    {
        var (notificationTemplateDto, error) = await _notificationTemplateServices.Create(form);
        if (error != null) return BadRequest(new { error });
        return Ok(notificationTemplateDto);
    }
    
    [HttpPut("update-notification-template/{id}")]
    public async Task<IActionResult> UpdateNotificationTemplate(Guid id, [FromBody] NotificationTemplateUpdateForm form)
    {
        var (notificationTemplateDto, error) = await _notificationTemplateServices.Update(id, form);
        if (error != null) return BadRequest(new { error });
        return Ok(notificationTemplateDto);
    }
    
    [HttpDelete("delete-notification-template/{id}")]
    public async Task<IActionResult> DeleteNotificationTemplate(Guid id)
    {
        var (isDeleted, error) = await _notificationTemplateServices.Delete(id);
        if (error != null) return BadRequest(new { error });
        return Ok(isDeleted);
    }
    
    [HttpGet("get-notification-template/{id}")]
    public async Task<IActionResult> GetNotificationTemplateById(Guid id)
    {
        var (notificationTemplateDto, error) = await _notificationTemplateServices.GetById(id);
        if (error != null) return BadRequest(new { error });
        return Ok(notificationTemplateDto);
    }
    
    [HttpGet("get-all-notification-templates")]
    public async Task<IActionResult> GetAllNotificationTemplates([FromQuery] NotificationTemplateFilterForm filter)
    {
        var (notificationTemplates, totalCount, error) = await _notificationTemplateServices.GetAll(filter);
        if (error != null) return BadRequest(new { error });
        var result = new Page<NotificationTemplateDto>()
        {
            Data = notificationTemplates,
            PagesCount = (int)Math.Ceiling((double)(totalCount ?? 0) / filter.PageSize),
            CurrentPage = filter.PageNumber,
            TotalCount = totalCount
        };
        return Ok(result);
    }
    
    [HttpGet("get-all-notification-types")]
    public async Task<IActionResult> GetAllNotificationTypes()
    {
        var notificationTypes = _enumTranslationService.GetAllEnumTranslations<NotificationType>();
        var response = new { data= notificationTypes };
        return Ok(response);
    }
    
    [HttpGet("get-all-notification-templates-status")]
    public async Task<IActionResult> GetAllNotificationTemplatesStatus()
    {
        var notificationTemplatesStatus = _enumTranslationService.GetAllEnumTranslations<NotificationStatus>();
        var response = new { data= notificationTemplatesStatus };
        return Ok(response);
    }
    
}