using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VipProjectV0._1.Db;
using VipTest.AirPortServices.Dto;
using VipTest.AirPortServices.models;
using VipTest.AirPortServices.Payloads;
using VipTest.AirPortServices.Utli;
using VipTest.attachmentsConfig;
using VipTest.Localization;
using VipTest.Notifications;
using VipTest.Rides.Utli;

namespace VipTest.AirPortServices.Services;

public interface IAirportServices
{
    Task<(AirPortServicesDto? airport, string? error)> Get(Guid id);
    Task<(List<AirPortServicesDto>? airport, int? totalCount, string? error)> GetAll(AirportServicesFilterForm filter);

    Task<(AirPortServicesDto? airport, string? error)> CreateVisaService(
        VisaVipServiceCreateForm from);

    Task<(AirPortServicesDto? airport, string? error)> CreateLuggageService(
        LuggageServiceCreateForm form);

    Task<(AirPortServicesDto? airport, string? error)> CreateLoungeService(
        LoungeServiceCreateForm form);

    Task<(AirPortServicesDto? airport, string? error)> UpdateVisaService(Guid id,
        VisaVipUpdateForm form);

    Task<(AirPortServicesDto? airport, string? error)> UpdateLuggageService(Guid id,
        LuggageUpdateForm form);

    Task<(AirPortServicesDto? airport, string? error)> UpdateLoungeService(Guid id,
        LoungeUpdateForm form);

    Task<(AirPortServicesDto? airport, string? error)> Start(Guid id);
    Task<(AirPortServicesDto? airport, string? error)> Reject(Guid id, string? reason);
    Task<(AirPortServicesDto? airport, string? error)> Cancel(Guid id, string? reason);
    Task<(AirPortServicesDto? airport, string? error)> Complete(Guid id);
    
    Task<(AirPortServicesDto? airport, string? error)> UpdateStatus(Guid id, AirportServicesStatus status,string? reason);
}

public class AirportServices : IAirportServices
{
    private readonly IRepositoryWrapper _repo;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localize;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserNotificationServices _notification;
    private readonly IDtoTranslationService _dtoTranslationService;

    public AirportServices(IRepositoryWrapper repo, IMapper mapper, ILocalizationService localize,
        IHttpContextAccessor httpContextAccessor, IUserNotificationServices notification,
        IDtoTranslationService dtoTranslationService)
    {
        _repo = repo;
        _mapper = mapper;
        _localize = localize;
        _httpContextAccessor = httpContextAccessor;
        _notification = notification;
        _dtoTranslationService = dtoTranslationService;
    }

    public async Task<(AirPortServicesDto? airport, string? error)> Get(Guid id)
    {
        var airport = await _repo.AirportServicesRepoistory.Get(
            x => x.Id == id,
            include: x => x.Include(x => x.Customer)
                .Include(x => x.Discount)
        );

        if (airport == null)
        {
            return (null, _localize.GetLocalizedString("AirportServiceNotFound"));
        }

        if (airport.Type == AirportServicesTypes.VipVisa)
        {
            var visaService = await _repo.VisaVipRepository.Get(
                x => x.Id == id,
                include: x => x.Include(x => x.Customer)
                    .Include(x => x.Discount)
            );
            if (visaService == null)
            {
                return (null, _localize.GetLocalizedString("AirportServiceNotFound"));
            }

            var visaServiceDto = _mapper.Map<VisaVipServiceDto>(visaService);
            visaServiceDto = _dtoTranslationService.TranslateEnums(visaServiceDto);
            return (visaServiceDto, null);
        }

        if (airport.Type == AirportServicesTypes.Luggage)
        {
            var luggageService = await _repo.LuggageRepository.Get(
                x => x.Id == id,
                include: x => x.Include(x => x.Customer)
                    .Include(x => x.Discount));
            if (luggageService == null)
            {
                return (null, _localize.GetLocalizedString("AirportServiceNotFound"));
            }

            var luggageServiceDto = _mapper.Map<LuggageServiceDto>(luggageService);
            luggageServiceDto = _dtoTranslationService.TranslateEnums(luggageServiceDto);
            return (luggageServiceDto, null);
        }

        if (airport.Type == AirportServicesTypes.Lounge)
        {
            var loungeService = await _repo.LoungeRepository.Get(
                x => x.Id == id,
                include: x => x.Include(x => x.Customer)
                    .Include(x => x.Discount));
            if (loungeService == null)
            {
                return (null, _localize.GetLocalizedString("AirportServiceNotFound"));
            }

            var loungeServiceDto = _mapper.Map<LoungeServiceDto>(loungeService);
            loungeServiceDto = _dtoTranslationService.TranslateEnums(loungeServiceDto);
            return (loungeServiceDto, null);
        }


        var airportDto = _mapper.Map<AirPortServicesDto>(airport);
        airportDto = _dtoTranslationService.TranslateEnums(airportDto);
        return (airportDto, null);
    }

    public async Task<(List<AirPortServicesDto>? airport, int? totalCount, string? error)> GetAll(
        AirportServicesFilterForm filter)
    {
        var (airports, totalCount) = await _repo.AirportServicesRepoistory.GetAll(
            x => (filter.CustomerId == null || x.CustomerId == filter.CustomerId) &&
                 (filter.PaymentStatus == null || x.PaymentStatus == filter.PaymentStatus) &&
                 (filter.PaymentType == null || x.PaymentType == filter.PaymentType) &&
                 (filter.Status == null || x.Status == filter.Status) &&
                 (filter.Type == null || x.Type == filter.Type),
            include: x => x.Include(x => x.Customer)
                .Include(x => x.Discount)
            ,
            filter.PageNumber, filter.PageSize);


        var airportDtos = _mapper.Map<List<AirPortServicesDto>>(airports);
        airportDtos = _dtoTranslationService.TranslateEnums(airportDtos);
        return (airportDtos, totalCount, null);
    }

    public async Task<(AirPortServicesDto? airport, string? error)> CreateVisaService(
        VisaVipServiceCreateForm form)
    {
        var settings = await _repo.SettingsRepository.Get(x => true);
        if (settings == null)
        {
            return (null, _localize.GetLocalizedString("SettingsNotFound"));
        }

        var visaService = _mapper.Map<VisaVipService>(form);

        var customer = await _repo.CustomerRepository.Get(x => x.Id == form.CustomerId);
        if (customer == null)
        {
            return (null, _localize.GetLocalizedString("CustomerNotFound"));
        }

        visaService.Customer = customer;
        visaService.CustomerId = customer.Id;
        visaService.Type = AirportServicesTypes.VipVisa;
        visaService.Status = AirportServicesStatus.Pending;
        visaService.PaymentStatus = PaymentStatus.UnPaid;
        visaService.Price = (decimal)(settings.VisaCommission* form.NumberOfCustomers);

        if (form.Attachments != null && form.Attachments.Count > 0)
        {
            visaService.Attachments = _mapper.Map<List<Attachments>>(form.Attachments);
        }

        if (!string.IsNullOrEmpty(form.DiscountCode))
        {
            var discount = await _repo.DiscountRepository.Get(x => x.Code == form.DiscountCode);
            if (discount != null && discount.IsValidForAirportService(customer.Id, DateTime.Now))
            {
                visaService.ApplyDiscount(discount);
                visaService.Discount = discount;
                visaService.DiscountId = discount.Id;
            }
        }
        else
        {
            visaService.FinalPrice = visaService.Price;
        }

        var service = await _repo.VisaVipRepository.Add(visaService);
        if (service == null)
        {
            return (null, _localize.GetLocalizedString("ServiceCreationFailed"));
        }

        var dto = _mapper.Map<VisaVipServiceDto>(visaService);
        dto = _dtoTranslationService.TranslateEnums(dto);
        return (dto, null);
    }

    public async Task<(AirPortServicesDto? airport, string? error)> CreateLuggageService(LuggageServiceCreateForm form)
    {
        var settings = await _repo.SettingsRepository.Get(x => true);
        if (settings == null)
        {
            return (null, _localize.GetLocalizedString("SettingsNotFound"));
        }

        var luggageService = _mapper.Map<LuggageService>(form);

        var customer = await _repo.CustomerRepository.Get(x => x.Id == form.CustomerId);
        if (customer == null)
        {
            return (null, _localize.GetLocalizedString("CustomerNotFound"));
        }
        
        if (form.NumberOfCustomers <= 0)
        {
            form.NumberOfCustomers = 1;
        }

        luggageService.Customer = customer;
        luggageService.CustomerId = customer.Id;
        luggageService.Type = AirportServicesTypes.Luggage;
        luggageService.Status = AirportServicesStatus.Pending;
        luggageService.PaymentStatus = PaymentStatus.UnPaid;
        luggageService.Price = (decimal)(settings.MissingBaggageCommission* form.NumberOfCustomers);

        if (!string.IsNullOrEmpty(form.DiscountCode))
        {
            var discount = await _repo.DiscountRepository.Get(x => x.Code == form.DiscountCode);
            if (discount != null && discount.IsValidForAirportService(customer.Id, DateTime.Now))
            {
                luggageService.ApplyDiscount(discount);
                luggageService.Discount = discount;
                luggageService.DiscountId = discount.Id;
            }
        }
        else
        {
            luggageService.FinalPrice = luggageService.Price;
        }

        var service = await _repo.LuggageRepository.Add(luggageService);
        if (service == null)
        {
            return (null, _localize.GetLocalizedString("ServiceCreationFailed"));
        }

        var dto = _mapper.Map<LuggageServiceDto>(luggageService);
        dto = _dtoTranslationService.TranslateEnums(dto);
        return (dto, null);
    }

    public async Task<(AirPortServicesDto? airport, string? error)> CreateLoungeService(LoungeServiceCreateForm form)
    {
        var settings = await _repo.SettingsRepository.Get(x => true);
        if (settings == null)
        {
            return (null, _localize.GetLocalizedString("SettingsNotFound"));
        }

        var loungeService = _mapper.Map<LoungeService>(form);

        var customer = await _repo.CustomerRepository.Get(x => x.Id == form.CustomerId);
        if (customer == null)
        {
            return (null, _localize.GetLocalizedString("CustomerNotFound"));
        }
        
        if (form.NumberOfCustomers <= 0)
        {
         form.NumberOfCustomers = 1;
        }

        loungeService.Customer = customer;
        loungeService.CustomerId = customer.Id;
        loungeService.Type = AirportServicesTypes.Lounge;
        loungeService.Status = AirportServicesStatus.Pending;
        loungeService.PaymentStatus = PaymentStatus.UnPaid;
        loungeService.Price = (decimal)(settings.VipLoungeCommission* form.NumberOfCustomers);

        if (!string.IsNullOrEmpty(form.DiscountCode))
        {
            var discount = await _repo.DiscountRepository.Get(x => x.Code == form.DiscountCode);
            if (discount != null && discount.IsValidForAirportService(customer.Id, DateTime.Now))
            {
                loungeService.ApplyDiscount(discount);
                loungeService.Discount = discount;
                loungeService.DiscountId = discount.Id;
            }
        }
        else
        {
            loungeService.FinalPrice = loungeService.Price;
        }

        var service = await _repo.LoungeRepository.Add(loungeService);
        if (service == null)
        {
            return (null, _localize.GetLocalizedString("ServiceCreationFailed"));
        }

        var dto = _mapper.Map<LoungeServiceDto>(loungeService);
        dto = _dtoTranslationService.TranslateEnums(dto);
        return (dto, null);
    }

    public async Task<(AirPortServicesDto? airport, string? error)> UpdateVisaService(Guid id, VisaVipUpdateForm form)
    {
        var visaService = await _repo.VisaVipRepository.Get(x => x.Id == id,
            include: x => x.Include(x => x.Customer)
                .Include(x => x.Discount)
        );
        if (visaService == null)
        {
            return (null, _localize.GetLocalizedString("ServiceNotFound"));
        }

        _mapper.Map(form, visaService);

        if (string.IsNullOrEmpty(form.DiscountCode))
        {
            var discount = await _repo.DiscountRepository.Get(x => x.Code == form.DiscountCode);
            if (discount != null && discount.IsValidForAirportService(visaService.CustomerId, DateTime.Now))
            {
                visaService.ApplyDiscount(discount);
                visaService.Discount = discount;
                visaService.DiscountId = discount.Id;
            }
        }

        if (form.Attachments != null && form.Attachments.Count > 0)
        {
            visaService.Attachments.RemoveAll(x => x.Id != null);
            visaService.Attachments = _mapper.Map<List<Attachments>>(form.Attachments);
        }

        var service = await _repo.VisaVipRepository.Update(visaService, id);
        if (service == null)
        {
            return (null, _localize.GetLocalizedString("ServiceUpdateFailed"));
        }

        var dto = _mapper.Map<VisaVipServiceDto>(visaService);
        dto = _dtoTranslationService.TranslateEnums(dto);
        return (dto, null);
    }

    public async Task<(AirPortServicesDto? airport, string? error)> UpdateLuggageService(Guid id,
        LuggageUpdateForm form)
    {
        var luggageService = await _repo.LuggageRepository.Get(x => x.Id == id,
            include: x => x.Include(x => x.Customer)
                .Include(x => x.Discount)
        );

        if (luggageService == null)
        {
            return (null, _localize.GetLocalizedString("ServiceNotFound"));
        }

        _mapper.Map(form, luggageService);

        if (string.IsNullOrEmpty(form.DiscountCode))
        {
            var discount = await _repo.DiscountRepository.Get(x => x.Code == form.DiscountCode);
            if (discount != null && discount.IsValidForAirportService(luggageService.CustomerId, DateTime.Now))
            {
                luggageService.ApplyDiscount(discount);
                luggageService.Discount = discount;
                luggageService.DiscountId = discount.Id;
            }
        }

        var service = await _repo.LuggageRepository.Update(luggageService, id);

        if (service == null)
        {
            return (null, _localize.GetLocalizedString("ServiceUpdateFailed"));
        }

        var dto = _mapper.Map<LuggageServiceDto>(luggageService);

        dto = _dtoTranslationService.TranslateEnums(dto);

        return (dto, null);
    }

    public async Task<(AirPortServicesDto? airport, string? error)> UpdateLoungeService(Guid id, LoungeUpdateForm form)
    {
        var loungeService = await _repo.LoungeRepository.Get(x => x.Id == id,
            include: x => x.Include(x => x.Customer)
                .Include(x => x.Discount)
        );

        if (loungeService == null)
        {
            return (null, _localize.GetLocalizedString("ServiceNotFound"));
        }

        _mapper.Map(form, loungeService);

        if (string.IsNullOrEmpty(form.DiscountCode))
        {
            var discount = await _repo.DiscountRepository.Get(x => x.Code == form.DiscountCode);
            if (discount != null && discount.IsValidForAirportService(loungeService.CustomerId, DateTime.Now))
            {
                loungeService.ApplyDiscount(discount);
                loungeService.Discount = discount;
                loungeService.DiscountId = discount.Id;
            }
        }

        var service = await _repo.LoungeRepository.Update(loungeService, id);

        if (service == null)
        {
            return (null, _localize.GetLocalizedString("ServiceUpdateFailed"));
        }

        var dto = _mapper.Map<LoungeServiceDto>(loungeService);

        dto = _dtoTranslationService.TranslateEnums(dto);
        
        return (dto, null);
    }

    public async Task<(AirPortServicesDto? airport, string? error)> Start(Guid id)
    {
        var service = await _repo.AirportServicesRepoistory.Get(x => x.Id == id,
            include: x => x.Include(x => x.Customer)
                .Include(x => x.Discount)
        );

        if (service == null)
        {
            return (null, _localize.GetLocalizedString("ServiceNotFound"));
        }

        service.Status = AirportServicesStatus.InProgress;

        var updatedService = await _repo.AirportServicesRepoistory.Update(service, id);

        if (updatedService == null)
        {
            return (null, _localize.GetLocalizedString("ServiceUpdateFailed"));
        }

        var dto = _mapper.Map<AirPortServicesDto>(updatedService);
        dto = _dtoTranslationService.TranslateEnums(dto);
        return (dto, null);
    }

    public async Task<(AirPortServicesDto? airport, string? error)> Reject(Guid id, string? reason)
    {
        var service = await _repo.AirportServicesRepoistory.Get(x => x.Id == id,
            include: x => x.Include(x => x.Customer)
                .Include(x => x.Discount)
        );

        if (service == null)
        {
            return (null, _localize.GetLocalizedString("ServiceNotFound"));
        }

        service.Status = AirportServicesStatus.Rejected;

        if (!string.IsNullOrEmpty(reason))
        {
            service.RejectReason = reason;
        }

        var updatedService = await _repo.AirportServicesRepoistory.Update(service, id);

        if (updatedService == null)
        {
            return (null, _localize.GetLocalizedString("ServiceUpdateFailed"));
        }

        var dto = _mapper.Map<AirPortServicesDto>(updatedService);

        dto = _dtoTranslationService.TranslateEnums(dto);

        return (dto, null);
    }

    public async Task<(AirPortServicesDto? airport, string? error)> Cancel(Guid id, string? reason)
    {
        var service = await _repo.AirportServicesRepoistory.Get(x => x.Id == id,
            include: x => x.Include(x => x.Customer)
                .Include(x => x.Discount)
        );

        if (service == null)
        {
            return (null, _localize.GetLocalizedString("ServiceNotFound"));
        }

        service.Status = AirportServicesStatus.Canceled;

        if (!string.IsNullOrEmpty(reason))
        {
            service.CancelReason = reason;
        }

        var updatedService = await _repo.AirportServicesRepoistory.Update(service, id);

        if (updatedService == null)
        {
            return (null, _localize.GetLocalizedString("ServiceUpdateFailed"));
        }

        var dto = _mapper.Map<AirPortServicesDto>(updatedService);

        dto = _dtoTranslationService.TranslateEnums(dto);

        return (dto, null);
    }

    public async Task<(AirPortServicesDto? airport, string? error)> Complete(Guid id)
    {
        var service = await _repo.AirportServicesRepoistory.Get(x => x.Id == id,
            include: x => x.Include(x => x.Customer)
                .Include(x => x.Discount)
        );
        if (service == null)
        {
            return (null, _localize.GetLocalizedString("ServiceNotFound"));
        }
        service.Status = AirportServicesStatus.Completed;
        var updatedService = await _repo.AirportServicesRepoistory.Update(service, id);
        if (updatedService == null)
        {
            return (null, _localize.GetLocalizedString("ServiceUpdateFailed"));
        }
        var dto = _mapper.Map<AirPortServicesDto>(updatedService);
        dto = _dtoTranslationService.TranslateEnums(dto);
        return (dto, null);
    }

    public async Task<(AirPortServicesDto? airport, string? error)> UpdateStatus(Guid id, AirportServicesStatus status,string? reason)
    {
        var service = await _repo.AirportServicesRepoistory.Get(x => x.Id == id,
            include: x => x.Include(x => x.Customer)
                .Include(x => x.Discount)
        );
        
        if (service == null)
        {
            return (null, _localize.GetLocalizedString("ServiceNotFound"));
        }

        switch (status)
        {
            case AirportServicesStatus.InProgress:
              return await Start(id);
            case AirportServicesStatus.Rejected:
                return await Reject(id, reason);
            case AirportServicesStatus.Canceled:
                return await Cancel(id, reason);
            case AirportServicesStatus.Completed:
                return await Complete(id);
        }
        
        return (null, _localize.GetLocalizedString("ServiceUpdateFailed"));
    }
}