using AutoMapper;
using VipTest.Transactions.dto;
using VipTest.Transactions.models;
using VipTest.Transactions.Payloads;

namespace VipTest.Transactions.mapper;

 public class TransactionMapper : Profile
    {
        public TransactionMapper()
        {
            // Mapping from Transaction to TransactionDto
            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => src.PaymentType))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.ServiceType, opt => opt.MapFrom(src => src.ServiceType))
                .ForMember(dest => dest.TransactionCode, opt => opt.MapFrom(src => src.TransactionCode))
                .ForMember(dest => dest.Ride, opt => opt.MapFrom(src => src.Ride))
                .ForMember(dest => dest.CarRental, opt => opt.MapFrom(src => src.CarRental));

            
            // Mapping from Transaction to TransactionDtoForInfo
            CreateMap<Transaction, TransactionDtoForInfo>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => src.PaymentType))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.ServiceType, opt => opt.MapFrom(src => src.ServiceType))
                .ForMember(dest => dest.TransactionCode, opt => opt.MapFrom(src => src.TransactionCode))
                .ForMember(dest => dest.FromWalletName, opt => opt.MapFrom(src => src.FromWallet.User.Username))
                .ForMember(dest => dest.ToWalletName, opt => opt.MapFrom(src => src.ToWallet.User.Username))
                .ForMember(dest => dest.RideCode, opt => opt.MapFrom(src => src.Ride.RidingCode))
                .ForMember(dest => dest.CarRentalCode, opt => opt.MapFrom(src => src.CarRental.OrderCode));
            
            
            
            
            // Mapping from TransactionDto to Transaction
            CreateMap<TransactionDto, Transaction>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => src.PaymentType))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));

            // Mapping from TransactionFilterForm to Transaction (used for filtering purposes)
            CreateMap<TransactionFilterForm, Transaction>()
                .ForMember(dest => dest.TransactionCode, opt => opt.Condition(src => !string.IsNullOrEmpty(src.TransactionCode))) // Only map if not null
                .ForMember(dest => dest.Amount, opt => opt.Condition(src => src.Amount.HasValue))
                .ForMember(dest => dest.PaymentType, opt => opt.Condition(src => src.PaymentType.HasValue))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Map non-null values only
        }
    }
