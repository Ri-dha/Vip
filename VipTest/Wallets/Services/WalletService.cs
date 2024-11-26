using AutoMapper;
using VipProjectV0._1.Db;
using VipTest.Localization;
using VipTest.Notifications;
using VipTest.Wallets.Dtos;
using VipTest.Wallets.Model;
using VipTest.Wallets.PayLoads;

namespace VipTest.Wallets.Services;

public interface IWalletService
{
    Task<(WalletDto dto, string? error)> CreateWalletAsync(CreateWalletForm form);
    Task<(WalletDto dto, string? error)> GetWalletAsync(Guid id);
    Task<(List<WalletDto> dtos, int? totalCount, string? error)> GetWalletsAsync(WalletFilterForm form);
}

public class WalletService : IWalletService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localize;
    private readonly IDtoTranslationService _dtoTranslationService;

    public WalletService(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILocalizationService localize,
        IDtoTranslationService dtoTranslationService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _localize = localize;
        _dtoTranslationService = dtoTranslationService;
    }

    public async Task<(WalletDto dto, string? error)> CreateWalletAsync(CreateWalletForm form)
    {
        var wallet = _mapper.Map<Wallet>(form);

        if (form.UserId != null)
        {
            var user = await _repositoryWrapper.UserRepository.Get(x => x.Id == form.UserId);
            if (user == null)
            {
                return (null, _localize.GetLocalizedString("UserNotFound"));
            }

            wallet.User = user;
            wallet.UserId = user.Id;

            user.Wallets.Add(wallet);
            await _repositoryWrapper.UserRepository.Update(user, user.Id);
        }

        wallet.Balance = 0;
        wallet.TotalIncome = 0;
        wallet.TotalExpense = 0;
        var result = await _repositoryWrapper.WalletRepository.Add(wallet);
        if (result == null)
        {
            return (null, _localize.GetLocalizedString("FailedToCreateWallet"));
        }

        return (_mapper.Map<WalletDto>(result), null);
    }

    public async Task<(WalletDto dto, string? error)> GetWalletAsync(Guid id)
    {
        var wallet = await _repositoryWrapper.WalletRepository.Get(x => x.Id == id);
        if (wallet == null)
        {
            return (null, _localize.GetLocalizedString("WalletNotFound"));
        }

        return (_mapper.Map<WalletDto>(wallet), null);
    }

    public async Task<(List<WalletDto> dtos, int? totalCount, string? error)> GetWalletsAsync(WalletFilterForm form)
    {
        var (wallets, totalCount) = await _repositoryWrapper.WalletRepository.GetAll<WalletDto>(
            x => (string.IsNullOrEmpty(form.Name) || x.Name.Contains(form.Name)) &&
                 (form.UserId == null || x.UserId == form.UserId) &&
                 (form.BalanceFrom == null || x.Balance >= form.BalanceFrom) &&
                 (form.BalanceTo == null || x.Balance <= form.BalanceTo) &&
                 (string.IsNullOrEmpty(form.Username) || x.User.Username.Contains(form.Username)) &&
                 (string.IsNullOrEmpty(form.PhoneNumber) || x.User.PhoneNumber.Contains(form.PhoneNumber))
            ,
            form.PageNumber, form.PageSize);
        return (_mapper.Map<List<WalletDto>>(wallets), totalCount, null);
    }
}