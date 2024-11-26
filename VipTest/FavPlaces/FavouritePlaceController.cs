using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VipTest.FavPlaces.Payloads;
using VipTest.FavPlaces.Utli;
using VipTest.Localization;
using VipTest.Utlity.Basic;

namespace VipTest.FavPlaces;

[Route("/favourite-places")]
public class FavouritePlaceController:BaseController
{
    
    private readonly IFavouritePlaceService _favouritePlaceService;
    private readonly IEnumTranslationService _enumTranslationService;

    public FavouritePlaceController(IFavouritePlaceService favouritePlaceService, IEnumTranslationService enumTranslationService)
    {
        _favouritePlaceService = favouritePlaceService;
        _enumTranslationService = enumTranslationService;
    }
    
    [HttpPost("create-favourite-place")]
    [SwaggerOperation(
        Summary = "Creates a new favorite place for a customer.",
        Description = "This endpoint allows creating a new favorite place associated with a customer. " +
                      "Note: Some fields are nullable, as indicated in the schema."
    )]
    public async Task<IActionResult> Create([FromBody] FavouritePlaceCreateForm favouritePlaceCreateForm)
    {
        var (favouritePlaceDto, error) = await _favouritePlaceService.AddFavouritePlace(favouritePlaceCreateForm);
        if (error != null) return BadRequest(new {error});
        return Ok(favouritePlaceDto);
    }
    
    [HttpPut("update-favourite-place/{id}")]
    [SwaggerOperation(
        Summary = "Updates an existing favorite place.",
        Description = "This endpoint allows updating an existing favorite place. " +
                      "Note: Some fields are nullable, as indicated in the schema."
    )]
    public async Task<IActionResult> Update(Guid id, [FromBody] FavouritePlaceUpdateForm favouritePlaceUpdateForm)
    {
        var (favouritePlaceDto, error) = await _favouritePlaceService.UpdateFavouritePlace(id, favouritePlaceUpdateForm);
        if (error != null) return BadRequest(new {error});
        return Ok(favouritePlaceDto);
    }
    
    [HttpDelete("delete-favourite-place/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var (favouritePlaceDto, error) = await _favouritePlaceService.DeleteFavouritePlace(id);
        if (error != null) return BadRequest(new {error});
        return Ok(favouritePlaceDto);
    }
    
    [HttpGet("get-favourite-place/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var (favouritePlaceDto, error) = await _favouritePlaceService.GetFavouritePlace(id);
        if (error != null) return BadRequest(new {error});
        return Ok(favouritePlaceDto);
    }
    
    [HttpGet("get-favourite-places-by-customer-id/{customerId}")]
    public async Task<IActionResult> GetFavouritePlacesByCustomerId(Guid customerId)
    {
        var (favouritePlaceDtos, totalCount, error) = await _favouritePlaceService.GetFavouritePlacesByCustomerId(customerId);
        if (error != null) return BadRequest(new {error});
        return Ok(new {data = favouritePlaceDtos, totalCount});
    
    }
    
    [HttpGet("get-place-types")]
    public IActionResult GetPlaceTypeTranslations()
    {
        var placeTypes = _enumTranslationService.GetAllEnumTranslations<PlaceType>();
        return Ok(placeTypes);
    }
    
    
}