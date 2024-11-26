using AutoMapper;
using VipProjectV0._1.Db;
using VipTest.Discounts.Dto;
using VipTest.Discounts.Models;
using VipTest.Discounts.Payloads;
using VipTest.Localization;
using VipTest.Rides.Utli;

namespace VipTest.Discounts;

public interface IDiscountService
{
    Task<(DiscountDto? discountDto, string? error)> CreateDiscountAsync(DiscountCreateForm createForm);
    Task<(DiscountDto? discountDto, string? error)> UpdateDiscountAsync(Guid id, DiscountUpdateForm updateForm);
    Task<(bool?, string? error)> DeleteDiscountAsync(Guid discountId);
    Task<(DiscountDto? discountDto, string? error)> GetDiscountAsync(Guid discountId);
    Task<(List<DiscountDto>? discountDtos, int total, string? error)> GetDiscountsAsync(DiscountFilterForm filterForm);

    Task<(DiscountDto? discountDto, string? error)> CheckDiscountAsync(string discountCode, Guid userId,
        DateTime currentDate, RideType rideType);

    Task<(bool success, string? error)> AddCustomersToDiscountAsync(Guid discountId, List<Guid> customerIds);
    Task<(bool success, string? error)> RemoveCustomersFromDiscountAsync(Guid discountId, List<Guid> customerIds);
}

public class DiscountService : IDiscountService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localize;

    public DiscountService(IMapper mapper, IRepositoryWrapper repositoryWrapper, ILocalizationService localize)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _localize = localize;
    }

    public async Task<(DiscountDto? discountDto, string? error)> CreateDiscountAsync(DiscountCreateForm createForm)
    {
        if (createForm.IsGlobal == null)
        {
            createForm.IsGlobal = false;
        }

        // Validation: if IsGlobal is true, ApplicableUserIds should be null or empty
        if (createForm.IsGlobal != null && createForm.IsGlobal == true)
        {
            createForm.ApplicableUserIds = null; // Reset the list for global discounts
        }

        var discount = _mapper.Map<Discount>(createForm);
        var addedDiscount = await _repositoryWrapper.DiscountRepository.Add(discount);

        if (addedDiscount == null)
        {
            return (null, _localize.GetLocalizedString("DiscountNotAdded"));
        }

        return (_mapper.Map<DiscountDto>(addedDiscount), null);
    }


    public async Task<(DiscountDto? discountDto, string? error)> UpdateDiscountAsync(Guid id,
        DiscountUpdateForm updateForm)
    {
        var discount = await _repositoryWrapper.DiscountRepository.GetById(id);
        if (discount == null)
        {
            return (null, _localize.GetLocalizedString("DiscountNotFound"));
        }

        // Map the updated values from the form
        _mapper.Map(updateForm, discount);

        // Ensure that user usage counts aren't affected when updating the limit
        if (updateForm.UsageLimit.HasValue)
        {
            discount.UsageLimitPerUser = updateForm.UsageLimit.Value;
        }

        var updatedDiscount = await _repositoryWrapper.DiscountRepository.Update(discount, id);
        if (updatedDiscount == null)
        {
            return (null, _localize.GetLocalizedString("DiscountNotUpdated"));
        }

        return (_mapper.Map<DiscountDto>(updatedDiscount), null);
    }


    public async Task<(bool?, string? error)> DeleteDiscountAsync(Guid discountId)
    {
        var discount = await _repositoryWrapper.DiscountRepository.GetById(discountId);
        if (discount == null)
        {
            return (false, _localize.GetLocalizedString("DiscountNotFound"));
        }

        discount.Deleted = true;
        await _repositoryWrapper.DiscountRepository.Update(discount, discountId);
        return (true, null);
    }

    public async Task<(DiscountDto? discountDto, string? error)> GetDiscountAsync(Guid discountId)
    {
        var discount = await _repositoryWrapper.DiscountRepository.GetById(discountId);
        if (discount == null)
        {
            return (null, _localize.GetLocalizedString("DiscountNotFound"));
        }

        return (_mapper.Map<DiscountDto>(discount), null);
    }

    public async Task<(List<DiscountDto>? discountDtos, int total, string? error)> GetDiscountsAsync(
        DiscountFilterForm filterForm)
    {
        var (discountDtos, totalCount) = await _repositoryWrapper.DiscountRepository.GetAll<DiscountDto>(
            x =>
                (filterForm.StartDate == null || x.StartDate >= filterForm.StartDate) &&
                (filterForm.EndDate == null || x.EndDate <= filterForm.EndDate) &&
                (!filterForm.IsPercentage || x.Percentage != null) &&
                (filterForm.Services == null || filterForm.Services.Any(s => x.DiscountServices.Contains(s))) &&
                x.Deleted == false
            ,
            filterForm.PageNumber,
            filterForm.PageSize);
        return (discountDtos, totalCount, null);
    }

    public async Task<(DiscountDto? discountDto, string? error)> CheckDiscountAsync(string discountCode, Guid userId,
        DateTime currentDate, RideType rideType)
    {
        var discount = await _repositoryWrapper.DiscountRepository.Get(
            x => x.Code == discountCode);
        if (discount == null)
        {
            return (null, _localize.GetLocalizedString("DiscountNotFound"));
        }

        if (!discount.IsValidForRide(userId, currentDate, rideType))
        {
            return (null, _localize.GetLocalizedString("DiscountNotValid"));
        }

        return (_mapper.Map<DiscountDto>(discount), null);
    }

    public async Task<(bool success, string? error)> AddCustomersToDiscountAsync(Guid discountId,
        List<Guid> customerIds)
    {
        var discount = await _repositoryWrapper.DiscountRepository.GetById(discountId);
        if (discount == null)
        {
            return (false, _localize.GetLocalizedString("DiscountNotFound"));
        }

        foreach (var customerId in customerIds)
        {
            if (!discount.ApplicableUserIds.Contains(customerId))
            {
                discount.ApplicableUserIds.Add(customerId);
            }
        }

        var updatedDiscount = await _repositoryWrapper.DiscountRepository.Update(discount, discountId);
        if (updatedDiscount == null)
        {
            return (false, _localize.GetLocalizedString("DiscountNotUpdated"));
        }

        return (true, null);
    }

    public async Task<(bool success, string? error)> RemoveCustomersFromDiscountAsync(Guid discountId,
        List<Guid> customerIds)
    {
        var discount = await _repositoryWrapper.DiscountRepository.GetById(discountId);
        if (discount == null)
        {
            return (false, _localize.GetLocalizedString("DiscountNotFound"));
        }

        foreach (var customerId in customerIds)
        {
            if (discount.ApplicableUserIds.Contains(customerId))
            {
                discount.ApplicableUserIds.Remove(customerId);
            }
        }

        var updatedDiscount = await _repositoryWrapper.DiscountRepository.Update(discount, discountId);
        if (updatedDiscount == null)
        {
            return (false, _localize.GetLocalizedString("DiscountNotUpdated"));
        }

        return (true, null);
    }
}