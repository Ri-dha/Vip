using System.Security.Claims;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace VipTest.Utlity.Basic;


[Route("api/v1/[controller]")]
[ApiController]
// [EnableCors("AllowLocalhost")]
public class BaseController:ControllerBase
{
    protected Guid Id => Guid.TryParse(GetClaim(ClaimTypes.NameIdentifier), out var id) ? id : Guid.Empty;
    protected string Language =>  Request.Headers["Accept-Language"].ToString();

    protected string Role => GetClaim("Role");

    protected Guid? ParentId
    {
        get
        {
            var idString = GetClaim("ParentId");
            Guid? re;
            if (!string.Equals(idString, null, StringComparison.Ordinal) &&
                !string.Equals(idString, "null", StringComparison.Ordinal))
                re = Guid.Parse(idString);
            else
                re = null;
            return re;
        }
    }

    protected string MethodType => HttpContext.Request.Method;

    protected virtual string GetClaim(string claimName)
    {
        var claims = (User.Identity as ClaimsIdentity)?.Claims;
        var claim = claims?.FirstOrDefault(c =>
            string.Equals(c.Type, claimName, StringComparison.CurrentCultureIgnoreCase) &&
            !string.Equals(c.Type, "null", StringComparison.CurrentCultureIgnoreCase));
        var rr = claim?.Value!.Replace("\"", "");

        return rr ?? "";
    }


    protected ObjectResult OkObject<T>((T? data, string? error) result)
    {
        return result.error != null
            ? base.BadRequest(new { Message = result.error })
            : base.Ok(result.data);
    }

   protected ObjectResult Ok<T>((List<T>? data, int? totalCount, string? error) result, int pageNumber, int pageSize = 10)
{
    if (result.error != null)
    {
        return base.BadRequest(new { Message = result.error });
    }

    // Determine the number of pages
    var pagesCount = pageSize == 0
        ? 1 // If pageSize is 0, there is only one page
        : (result.totalCount - 1) / pageSize + 1;

    return base.Ok(new Page<T>
    {
        Data = result.data,
        PagesCount = pagesCount,
        CurrentPage = pageNumber,
        
    });
}

    protected ObjectResult Ok<T>((T obj, string? error) result)
    {
        return result.error != null
            ? base.BadRequest(new { Message = result.error })
            : base.Ok(result.obj);
    }
}