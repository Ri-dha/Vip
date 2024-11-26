using Microsoft.AspNetCore.Mvc;
using VipTest.Discounts.utli;
using VipTest.FavPlaces.Utli;
using VipTest.Rides.Utli;
using VipTest.Users.Admins;
using VipTest.Users.customers;
using VipTest.Users.Drivers;
using VipTest.Users.Models;
using VipTest.Utlity;
using VipTest.Utlity.Basic;
using VipTest.vehicles.Utli;

namespace VipTest.Localization;

[Route("/Enums")]
public class EnumsController:BaseController
{
    private readonly IEnumTranslationService _enumTranslationService;

    public EnumsController(IEnumTranslationService enumTranslationService)
    {
        _enumTranslationService = enumTranslationService;
    }

    [HttpGet("get-all-enum-translations")]
    public IActionResult GetAllTranslations()
    {
        var allTranslations = new
        {
            DiscountServices = _enumTranslationService.GetAllEnumTranslations<DiscountServices>(),
            PlaceTypes = _enumTranslationService.GetAllEnumTranslations<PlaceType>(),
            RideTypes = _enumTranslationService.GetAllEnumTranslations<RideType>(),
            RideStatuses = _enumTranslationService.GetAllEnumTranslations<RideStatus>(),
            RidePaymentTypes = _enumTranslationService.GetAllEnumTranslations<RidePaymentType>(),
            DriverStatuses = _enumTranslationService.GetAllEnumTranslations<DriverStatus>(),
            AdminRoles = _enumTranslationService.GetAllEnumTranslations<AdministrativeRoles>(),
            Roles = _enumTranslationService.GetAllEnumTranslations<Roles>(),
            CustomerStatuses = _enumTranslationService.GetAllEnumTranslations<CustomerStatus>(),
            VehicleStatuses = _enumTranslationService.GetAllEnumTranslations<VehicleStatus>(),
            VehicleTypes = _enumTranslationService.GetAllEnumTranslations<VehicleType>(),
            Governorates = _enumTranslationService.GetAllEnumTranslations<IraqGovernorates>()
        };

        return Ok(allTranslations);
    }

    
}