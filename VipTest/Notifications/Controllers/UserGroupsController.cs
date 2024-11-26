using Microsoft.AspNetCore.Mvc;
using VipTest.Localization;
using VipTest.Notifications.Dtos;
using VipTest.Notifications.Models;
using VipTest.Notifications.PayLoads;
using VipTest.Utlity;
using VipTest.Utlity.Basic;

namespace VipTest.Notifications.Controllers;

[Route("user-groups")]
public class UserGroupsController:BaseController
{
    private readonly IEnumTranslationService _enumTranslationService;
    private readonly IUserGroupsServices _userGroupService;

    public UserGroupsController(IEnumTranslationService enumTranslationService, IUserGroupsServices userGroupService)
    {
        _enumTranslationService = enumTranslationService;
        _userGroupService = userGroupService;
    }
    
    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var (userGroup, error) = await _userGroupService.GetById(id);
        if (error != null) return BadRequest(new { error });
        return Ok(userGroup);
    }
    
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll([FromQuery] UserGroupsFilter filter)
    {
        var (userGroups, totalCount, error) = await _userGroupService.GetAll(filter);
        if (error != null) return BadRequest(new { error });

        var response = new Page<UserGroupsDto>()
        {
            Data = userGroups,
            PagesCount = (int)Math.Ceiling((double)(totalCount ?? 0) / filter.PageSize),
            CurrentPage = filter.PageNumber,
            TotalCount = totalCount
        };
        
        return Ok(response);
        
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] UserGroupsForm form)
    {
        var (userGroup, error) = await _userGroupService.Create(form);
        if (error != null) return BadRequest(new { error });
        return Ok(userGroup);
    }
    
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UserGroupsUpdateForm form)
    {
        var (userGroup, error) = await _userGroupService.Update(id, form);
        if (error != null) return BadRequest(new { error });
        return Ok(userGroup);
    }
    
    
}