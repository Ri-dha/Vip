using Microsoft.AspNetCore.Mvc;
using VipTest.AppSettings.Dto;
using VipTest.AppSettings.Payloads;
using VipTest.Utlity.Basic;

namespace VipTest.AppSettings;

[Route("settings")]
public class SettingsController:BaseController
{
    private readonly ISettingsService _settingsService;

    public SettingsController(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    [HttpGet("/get")]
    public async Task<IActionResult> GetSettings()
    {
        var settings = await _settingsService.GetByIdAsync();
        return Ok(settings);
    }

    [HttpPut("/update")]
    public async Task<ActionResult<SettingsDto>> UpdateSettings([FromBody]SettingsUpdateForm settingsFrom)
    {
        await _settingsService.UpdateAsync(settingsFrom);
        return Ok();
    }
    
}