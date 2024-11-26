using System.Transactions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VipProjectV0._1.Db;
using VipTest.Localization;
using VipTest.Rentals.Dto;
using VipTest.Rides.Dto;
using VipTest.Transactions.dto;
using VipTest.Transactions.models;
using VipTest.Transactions.Payloads;
using VipTest.Transactions.utli;

namespace VipTest.Transactions;

public interface ITransactionServices
{
    Task<(TransactionDto? dto, string? error)> UpdateTransactionStatus(Guid transactionId,
        TransactionPaymentStatus status);

    Task<(List<TransactionDto>? dtos, int? totalCount, string? error)>
        GetTransactions(TransactionFilterForm filterForm);

    Task<(TransactionDto? dto, string? error)> GetTransaction(Guid transactionId);
}

public class TransactionServices : ITransactionServices
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localize;
    private readonly IDtoTranslationService _dtoTranslationService;


    public TransactionServices(ILocalizationService localize, IMapper mapper, IRepositoryWrapper repositoryWrapper, IDtoTranslationService dtoTranslationService)
    {
        _localize = localize;
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _dtoTranslationService = dtoTranslationService;
    }


    public async Task<(TransactionDto? dto, string? error)> UpdateTransactionStatus(Guid transactionId,
        TransactionPaymentStatus status)
    {
        var transaction = await _repositoryWrapper.TransactionReposiotry.Get(
            x => x.Id == transactionId);
        if (transaction == null)
        {
            return (null, _localize.GetLocalizedString("TransactionNotFound"));
        }

        transaction.Status = status;
        var result = await _repositoryWrapper.TransactionReposiotry.Update(transaction, transactionId);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToUpdateTransactionStatus"));
        }

        return (_mapper.Map<TransactionDto>(result), null);
    }

    public async Task<(List<TransactionDto>? dtos, int? totalCount, string? error)> GetTransactions(
        TransactionFilterForm filterForm)
    {
        // Fetch transactions with filters applied
        var (transactions, totalCount) = await _repositoryWrapper.TransactionReposiotry.GetAll(
            x =>
                (string.IsNullOrEmpty(filterForm.TransactionCode) ||
                 x.TransactionCode.Contains(filterForm.TransactionCode)) &&
                (!filterForm.Status.HasValue || x.Status == filterForm.Status) &&
                (!filterForm.PaymentType.HasValue || x.PaymentType == filterForm.PaymentType.Value) &&
                (filterForm.ServiceType == null || filterForm.ServiceType.Contains(x.ServiceType)) &&
                (filterForm.Amount == null || x.Amount == filterForm.Amount) &&
                (filterForm.FromWalletId == null || x.FromWalletId == filterForm.FromWalletId) &&
                (filterForm.ToWalletId == null || x.ToWalletId == filterForm.ToWalletId) &&
                (filterForm.WarehouseId == null || x.WarehouseId == filterForm.WarehouseId)
            ,
            include: src => src
                .Include(x => x.Ride)
                .ThenInclude(x => x.Vehicle)
                .Include(x => x.CarRental)!
                .ThenInclude(c=>c.Vehicle)
                .ThenInclude(c=>c.Warehouse)
                .Include(x => x.FromWallet)
                .Include(x => x.ToWallet)
            ,
            filterForm.PageNumber,
            filterForm.PageSize
        );


        var dtos = _mapper.Map<List<TransactionDto>>(transactions);
        dtos.ForEach(x => x = _dtoTranslationService.TranslateEnums(x));
        return (dtos, totalCount, null);
    }


    public async Task<(TransactionDto? dto, string? error)> GetTransaction(Guid transactionId)
    {
        var transaction = await _repositoryWrapper.TransactionReposiotry.Get(
            x => x.Id == transactionId,
            include: source => source.Include(x => x.FromWallet)
                .Include(x => x.ToWallet)
                .Include(x => x.Ride)
                .Include(x => x.CarRental)
        );

        
        
        if (transaction == null)
        {
            return (null, _localize.GetLocalizedString("TransactionNotFound"));
        }

        var dto = _mapper.Map<TransactionDto>(transaction);
        
        if (transaction.CarRentalId != null)
        {
            var carRental = await _repositoryWrapper.CarRentalOrderRepository.Get(x => transaction.CarRentalId == x.Id,
                include: source => source
                    .Include(x => x.Customer)
                    .Include(x => x.Vehicle)
                    .Include(x => x.Vehicle.Warehouse)
            );

        var carRentDto = _mapper.Map<CarRentalOrderDto>(carRental);
        dto.CarRental = carRentDto;
        }
        else
        {
            var Ride = await _repositoryWrapper.RideRepository.Get(x => transaction.RideId == x.Id,
                include: source => source.Include(x => x.Customer)
                    .Include(x => x.Driver)
                    .Include(x => x.Vehicle)
            );
            
            var rideDto = _mapper.Map<RideDto>(Ride);
            dto.Ride = rideDto;
        }
        
        dto = _dtoTranslationService.TranslateEnums(dto);
        return (dto, null);
    }
}