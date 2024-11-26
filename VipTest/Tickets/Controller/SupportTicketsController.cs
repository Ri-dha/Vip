using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VipTest.Localization;
using VipTest.Tickets.Dto;
using VipTest.Tickets.Payload;
using VipTest.Tickets.Service;
using VipTest.Tickets.utli;
using VipTest.Utlity;
using VipTest.Utlity.Basic;

namespace VipTest.Tickets.Controller;

[Route("tickets")]
public class SupportTicketsController : BaseController
{
    private readonly ISupportTicketsService _supportTicketsService;
    private readonly IEnumTranslationService _enumTranslationService;

    public SupportTicketsController(ISupportTicketsService supportTicketsService, IEnumTranslationService enumTranslationService)
    {
        _supportTicketsService = supportTicketsService;
        _enumTranslationService = enumTranslationService;
    }

    [HttpPost("create")]
    [SwaggerOperation(
        Summary = "Creates a new support ticket.",
        Description = "This endpoint allows creating a new support ticket. " +
                      "Note: Some fields are nullable, as indicated in the schema."
    )]
    public async Task<ActionResult<SupportTicketsDto?>> Create(
        [FromBody] SupportTicketsCreateForm supportTicketsCreateForm)
    {
        var (supportTicketDto, error) = await _supportTicketsService.CreateSupportTicketAsync(supportTicketsCreateForm);
        if (error != null) return BadRequest(new { error });
        return Ok(supportTicketDto);
    }

    [HttpPut("update/{id}")]
    [SwaggerOperation(
        Summary = "Updates an existing support ticket.",
        Description = "This endpoint allows updating an existing support ticket. " +
                      "Note: Some fields are nullable, as indicated in the schema."
    )]
    public async Task<ActionResult<SupportTicketsDto>> Update(Guid id,
        [FromBody] SupportTicketsUpdateForm supportTicketsUpdateForm)
    {
        var (supportTicketDto, error) =
            await _supportTicketsService.UpdateSupportTicketAsync(id, supportTicketsUpdateForm);
        if (error != null) return BadRequest(new { error });
        return Ok(supportTicketDto);
    }
    
    [HttpPut("reply/{id}")]
    public async Task<ActionResult<SupportTicketsDto>> Reply(Guid id, [FromBody] SupportTicketReplyRequest response)
    {
        var (supportTicketDto, error) = await _supportTicketsService.ReplyToTicketAsync(id, response.Response);
        if (error != null) return BadRequest(new { error });
        return Ok(supportTicketDto);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var (supportTicketDto, error) = await _supportTicketsService.DeleteSupportTicketAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(supportTicketDto);
    }


    [HttpGet("get-all-tickets")]
    public async Task<ActionResult<Page<SupportTicketsDto>>>
        GetAllTickets([FromQuery] SupportTicketsFilterForm filter) =>
        Ok(await _supportTicketsService.GetAllTickets(filter), filter.PageNumber, filter.PageSize);

    [HttpGet("get/{id}")]
    public async Task<ActionResult<SupportTicketsDto>> Get(Guid id)
    {
        var (supportTicketDto, error) = await _supportTicketsService.GetSupportTicketAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(supportTicketDto);
    }

    [HttpPut("assign/{id}")]
    public async Task<IActionResult> Assign(Guid id, [FromBody] Guid adminId)
    {
        var (success, error) = await _supportTicketsService.AssignSupportTicketAsync(id, adminId);
        if (error != null) return BadRequest(new { error });
        return Ok(success);
    }

    [HttpPut("close/{id}")]
    public async Task<IActionResult> Close(Guid id)
    {
        var (success, error) = await _supportTicketsService.CloseSupportTicketAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(success);
    }

    [HttpGet("get-all-statuses")]
    public IActionResult GetTicketStatusTranslations()
    {
        var statuses = _enumTranslationService.GetAllEnumTranslations<TicketStatus>();
        return Ok(statuses);
    }

    
    [HttpGet("get-all-types")]
    public IActionResult GetTicketTypeTranslations()
    {
        var types = _enumTranslationService.GetAllEnumTranslations<TicketType>();
        return Ok(types);
    }

    
   
}