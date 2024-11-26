using AutoMapper;
using VipTest.Wallets.Dtos;
using VipTest.Wallets.Model;
using VipTest.Wallets.PayLoads;

namespace VipTest.Wallets.Mapper;

public class WalletMapper : Profile
{
    public WalletMapper()
    {
        // Map Wallet to WalletDto
        CreateMap<Wallet, WalletDto>()
            .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.Transactions));

        // Map WalletDto to Wallet (if needed, typically for reverse mapping scenarios)
        CreateMap<WalletDto, Wallet>()
            .ForMember(dest => dest.Transactions, opt => opt.Ignore());

        // Map CreateWalletForm to Wallet
        CreateMap<CreateWalletForm, Wallet>()
            .ForMember(dest => dest.Transactions, opt => opt.Ignore())
            .ForMember(dest => dest.Balance, opt => opt.Ignore())
            .ForMember(dest => dest.TotalIncome, opt => opt.Ignore())
            .ForMember(dest => dest.TotalExpense, opt => opt.Ignore());

        // Map WalletFilterForm -> Wallet (used for filtering, partial mapping)
        CreateMap<WalletFilterForm, Wallet>()
            .ForAllMembers(opt => opt.Ignore()); // Ignore all members since it's for filtering
    }
}