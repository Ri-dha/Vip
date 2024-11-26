using AutoMapper;
using VipTest.AirPortServices.repoistores;
using VipTest.AppSettings.repo;
using VipTest.Data;
using VipTest.DesignatedPlaces.repository;
using VipTest.Discounts.repositories;
using VipTest.FavPlaces.Repositories;
using VipTest.Files;
using VipTest.Notifications.Repositories;
using VipTest.Rentals.Repository;
using VipTest.reviews.Repositories;
using VipTest.RideBillings.Repositories;
using VipTest.Rides.Repositories;
using VipTest.Tickets.Repoistory;
using VipTest.Transactions.Repository;
using VipTest.Users.Admins;
using VipTest.Users.BranchManagers;
using VipTest.Users.customers;
using VipTest.Users.Drivers;
using VipTest.Users.OTP;
using VipTest.Users.Repositories;
using VipTest.Utlity;
using VipTest.vehicles.Repositories;
using VipTest.Wallets.Repositories;
using VipTest.Warehouses;

namespace VipProjectV0._1.Db;

public interface IRepositoryWrapper
{
    IUserRepository UserRepository { get; }
    IFileRepository FileRepository { get; }
    IDriverRepository DriverRepository { get; }
    ICustomerRepository CustomerRepository { get; }
    IAdminRepository AdminRepository { get; }
    IBranchManagerRepository BranchManagerRepository { get; }

    IVehiclesRepository VehiclesRepository { get; }
    IPendingCustomerRepository PendingCustomerRepository { get; }
    IWarehouseRepository WarehouseRepository { get; }
    IRideRepository RideRepository { get; }

    IFavouritePlaceRepository FavouritePlaceRepository { get; }

    IRideBillingRepository RideBillingRepository { get; }
    IDiscountRepository DiscountRepository { get; }
    ISupportTicketsReposiotry SupportTicketsReposiotry { get; }
    IDesignatedPlacesRepository DesignatedPlacesRepository { get; }
    ISettingsRepository SettingsRepository { get; }
    IReviewRepository ReviewRepository { get; }
    IDriverReviewRepository DriverReviewRepository { get; }
    IRideReviewRepository RideReviewRepository { get; }
    IVehicleReviewRepository VehicleReviewRepository { get; }
    ICarRentalOrderRepository CarRentalOrderRepository { get; }

    ITransactionReposiotry TransactionReposiotry { get; }

    IUserNotificationRepository UserNotificationRepository { get; }
    INotificationTemplateRepository NotificationTemplateRepository { get; }
    IUserGroupsRepository UserGroupsRepository { get; }

    IWalletRepository WalletRepository { get; }

    IAirportServicesRepoistory AirportServicesRepoistory { get; }
    ILuggageRepository LuggageRepository { get; }

    ILoungeRepository LoungeRepository { get; }

    IVisaVipRepository VisaVipRepository { get; }
}

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly VipProjectContext _repoContext;
    private readonly IMapper _mapper;
    private IUserRepository _userRepository;
    private IFileRepository _fileRepository;
    private IDriverRepository _driverRepository;
    private ICustomerRepository _customerRepository;
    private IAdminRepository _adminRepository;
    private IBranchManagerRepository _branchManagerRepository;
    private IVehiclesRepository _vehiclesRepository;
    private IPendingCustomerRepository _pendingCustomerRepository;
    private IWarehouseRepository _warehouseRepository;
    private IRideRepository _rideRepository;
    private IFavouritePlaceRepository _favouritePlaceRepository;
    private IRideBillingRepository _rideBillingRepository;
    private IDiscountRepository _discountRepository;
    private ISupportTicketsReposiotry _supportTicketsReposiotry;
    private IDesignatedPlacesRepository _designatedPlacesRepository;
    private ISettingsRepository _settingsRepository;
    private IReviewRepository _reviewRepository;
    private IRideReviewRepository _rideReviewRepository;
    private IDriverReviewRepository _driverReviewRepository;
    private IVehicleReviewRepository _vehicleReviewRepository;
    private ICarRentalOrderRepository _carRentalOrderRepository;
    private ITransactionReposiotry _transactionReposiotry;
    private IUserNotificationRepository _userNotificationRepository;
    private INotificationTemplateRepository _notificationTemplateRepository;
    private IUserGroupsRepository _userGroupsRepository;
    private IWalletRepository _walletRepository;
    private IAirportServicesRepoistory _airportServicesRepoistory;
    private ILuggageRepository _luggageRepository;
    private ILoungeRepository _loungeRepository;
    private IVisaVipRepository _visaVipRepository;


    public RepositoryWrapper(VipProjectContext repoContext, IMapper mapper)
    {
        _repoContext = repoContext;
        _mapper = mapper;
    }

    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_repoContext, _mapper);
    public IFileRepository FileRepository => _fileRepository ??= new FileRepository(_repoContext, _mapper);

    public IDriverRepository DriverRepository =>
        _driverRepository ??= new DriverRepository(_repoContext, _mapper);

    public ICustomerRepository CustomerRepository =>
        _customerRepository ??= new CustomerRepository(_repoContext, _mapper);

    public IAdminRepository AdminRepository =>
        _adminRepository ??= new AdminRepository(_repoContext, _mapper);

    public IBranchManagerRepository BranchManagerRepository =>
        _branchManagerRepository ??= new BranchManagerRepository(_repoContext, _mapper);

    public IVehiclesRepository VehiclesRepository =>
        _vehiclesRepository ??= new VehiclesRepository(_repoContext, _mapper);

    public IPendingCustomerRepository PendingCustomerRepository =>
        _pendingCustomerRepository ??= new PendingCustomerRepository(_repoContext, _mapper);

    public IWarehouseRepository WarehouseRepository =>
        _warehouseRepository ??= new WarehouseRepository(_repoContext, _mapper);

    public IRideRepository RideRepository =>
        _rideRepository ??= new RideRepository(_repoContext, _mapper);

    public IFavouritePlaceRepository FavouritePlaceRepository =>
        _favouritePlaceRepository ??= new FavouritePlaceRepository(_repoContext, _mapper);

    public IRideBillingRepository RideBillingRepository =>
        _rideBillingRepository ??= new RideBillingRepository(_repoContext, _mapper);

    public IDiscountRepository DiscountRepository =>
        _discountRepository ??= new DiscountRepository(_repoContext, _mapper);

    public ISupportTicketsReposiotry SupportTicketsReposiotry =>
        _supportTicketsReposiotry ??= new SupportTicketsReposiotry(_repoContext, _mapper);

    public IDesignatedPlacesRepository DesignatedPlacesRepository =>
        _designatedPlacesRepository ??= new DesignatedPlacesRepository(_repoContext, _mapper);

    public ISettingsRepository SettingsRepository =>
        _settingsRepository ??= new SettingsRepository(_repoContext, _mapper);

    public IReviewRepository ReviewRepository =>
        _reviewRepository ??= new ReviewRepository(_repoContext, _mapper);

    public IRideReviewRepository RideReviewRepository =>
        _rideReviewRepository ??= new RideReviewRepository(_repoContext, _mapper);

    public IDriverReviewRepository DriverReviewRepository =>
        _driverReviewRepository ??= new DriverReviewRepository(_repoContext, _mapper);

    public IVehicleReviewRepository VehicleReviewRepository =>
        _vehicleReviewRepository ??= new VehicleReviewRepository(_repoContext, _mapper);

    public ICarRentalOrderRepository CarRentalOrderRepository =>
        _carRentalOrderRepository ??= new CarRentalOrderRepository(_repoContext, _mapper);

    public ITransactionReposiotry TransactionReposiotry =>
        _transactionReposiotry ??= new TransactionReposiotry(_repoContext, _mapper);

    public IUserNotificationRepository UserNotificationRepository =>
        _userNotificationRepository ??= new UserNotificationRepository(_repoContext, _mapper);

    public INotificationTemplateRepository NotificationTemplateRepository =>
        _notificationTemplateRepository ??= new NotificationTemplateRepository(_repoContext, _mapper);

    public IUserGroupsRepository UserGroupsRepository =>
        _userGroupsRepository ??= new UserGroupsRepository(_repoContext, _mapper);

    public IWalletRepository WalletRepository =>
        _walletRepository ??= new WalletRepository(_repoContext, _mapper);

    public IAirportServicesRepoistory AirportServicesRepoistory =>
        _airportServicesRepoistory ??= new AirportServicesRepoistory(_repoContext, _mapper);

    public ILuggageRepository LuggageRepository =>
        _luggageRepository ??= new LuggageRepository(_repoContext, _mapper);

    public ILoungeRepository LoungeRepository =>
        _loungeRepository ??= new LoungeRepository(_repoContext, _mapper);

    public IVisaVipRepository VisaVipRepository =>
        _visaVipRepository ??= new VisaVipRepository(_repoContext, _mapper);
}