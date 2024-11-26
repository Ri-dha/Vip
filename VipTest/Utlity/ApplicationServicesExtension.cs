using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using VipProjectV0._1.Db;
using VipTest.AirPortServices.Mapper;
using VipTest.AirPortServices.repoistores;
using VipTest.AirPortServices.Services;
using VipTest.AppSettings;
using VipTest.AppSettings.Mapper;
using VipTest.AppSettings.models;
using VipTest.AppSettings.repo;
using VipTest.Auth;
using VipTest.DesignatedPlaces.mapper;
using VipTest.DesignatedPlaces.repository;
using VipTest.DesignatedPlaces.services;
using VipTest.Discounts;
using VipTest.Discounts.Mapper;
using VipTest.Discounts.Models;
using VipTest.Discounts.repositories;
using VipTest.FavPlaces;
using VipTest.FavPlaces.Mapper;
using VipTest.FavPlaces.Repositories;
using VipTest.Files.Serivce;
using VipTest.Localization;
using VipTest.Notifications;
using VipTest.Notifications.Mappers;
using VipTest.Notifications.Models;
using VipTest.Notifications.Repositories;
using VipTest.Rentals;
using VipTest.Rentals.Mapper;
using VipTest.Rentals.Models;
using VipTest.Rentals.Repository;
using VipTest.reviews;
using VipTest.reviews.Mappers;
using VipTest.reviews.Repositories;
using VipTest.reviews.utli;
using VipTest.RideBillings;
using VipTest.RideBillings.Mapper;
using VipTest.RideBillings.Repositories;
using VipTest.Rides;
using VipTest.Rides.Mapper;
using VipTest.Rides.Repositories;
using VipTest.Tickets.Mapper;
using VipTest.Tickets.Repoistory;
using VipTest.Tickets.Service;
using VipTest.Transactions;
using VipTest.Transactions.mapper;
using VipTest.Transactions.Repository;
using VipTest.Users.Admins;
using VipTest.Users.BranchManagers;
using VipTest.Users.customers;
using VipTest.Users.Drivers;
using VipTest.Users.Mappers;
using VipTest.Users.Models;
using VipTest.Users.OTP;
using VipTest.Users.services;
using VipTest.vehicles.Dtos;
using VipTest.vehicles.Mapper;
using VipTest.vehicles.Repositories;
using VipTest.vehicles.Services;
using VipTest.vehicles.Utli;
using VipTest.Wallets.Mapper;
using VipTest.Wallets.Repositories;
using VipTest.Wallets.Services;
using VipTest.Warehouses;
using VipTest.Warehouses.Mapper;
using VipTest.Warehouses.Services;

namespace VipTest.Utlity;

public static class ApplicationServicesExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserServices, UserServices>();
        services.AddScoped<IFileService, FileService>(); // Register the FileService
        services.AddScoped<IDriverRepository, DriverRepository>(); // Register the DriverRepository
        services.AddScoped<ICustomerRepository, CustomerRepository>(); // Register the CustomerRepository
        services.AddScoped<IAdminRepository, AdminRepository>(); // Register the AdminRepository
        services.AddScoped<IBranchManagerRepository, BranchManagerRepository>(); // Register the BranchManagerRepository
        services
            .AddScoped<IPendingCustomerRepository,
                PendingCustomerRepository>(); // Register the PendingCustomerRepository
        services.AddScoped<IVehiclesRepository, VehiclesRepository>(); // Register the VehiclesRepository
        services.AddScoped<IVehicleServices, VehicleServices>(); // Register the VehicleServices
        services.AddScoped<IWarehouseRepository, WarehouseRepository>(); // Register the WarehouseRepository
        services.AddScoped<IWarehouseServices, WarehouseServices>(); // Register the WarehouseServices
        services.AddScoped<IRideRepository, RideRepository>(); // Register the RideRepository
        services
            .AddScoped<IFavouritePlaceRepository, FavouritePlaceRepository>(); // Register the FavouritePlaceRepository
        services.AddScoped<IFavouritePlaceService, FavouritePlaceService>(); // Register the FavouritePlaceService
        services.AddScoped<IRideService, RideService>();
        services.AddScoped<IRideBillingRepository, RideBillingRepository>(); // Register the RideBillingRepository
        services
            .AddScoped<IRideBillingTypesConfigService,
                RideBillingTypesConfigService>(); // Register the RideBillingTypesConfigRepository
        services.AddScoped<IDiscountRepository, DiscountRepository>(); // Register the DiscountRepository
        services.AddScoped<IDiscountService, DiscountService>(); // Register the DiscountService'
        services
            .AddScoped<ISupportTicketsReposiotry, SupportTicketsReposiotry>(); // Register the SupportTicketsReposiotry
        services.AddScoped<ISupportTicketsService, SupportTicketsService>(); // Register the SupportTicketsService
        services
            .AddScoped<IDesignatedPlacesRepository,
                DesignatedPlacesRepository>(); // Register the DesignatedPlacesRepository
        services.AddScoped<IDesignatedPlacesService, DesignatedPlacesService>(); // Register the DesignatedPlacesService
        services.AddScoped<ISettingsRepository, SettingsRepository>();
        services.AddScoped<ISettingsService, SettingsService>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<ICarRentalOrderRepository, CarRentalOrderRepository>();
        services.AddScoped<ICarRentalOrderService, CarRentalOrderService>();
        services.AddScoped<ITransactionReposiotry, TransactionReposiotry>();
        services.AddScoped<ITransactionServices, TransactionServices>();
        services.AddScoped<IUserGroupsRepository, UserGroupsRepository>();
        services.AddScoped<IUserGroupsServices, UserGroupsServices>();
        services.AddScoped<INotificationTemplateRepository, NotificationTemplateRepository>();
        services.AddScoped<INotificationTemplateServices, NotificationTemplateServices>();
        services.AddScoped<IUserNotificationRepository, UserNotificationRepository>();
        services.AddScoped<IUserNotificationServices, UserNotificationServices>();
        services.AddScoped<IRentCarsServices, RentCarsServices>();
        services.AddScoped<IWalletService,WalletService>();
        services.AddScoped<IWalletRepository,WalletRepository>();
        services.AddScoped<IAirportServicesRepoistory,AirportServicesRepoistory>();
        services.AddScoped<ILuggageRepository,LuggageRepository>();
        services.AddScoped<ILoungeRepository,LoungeRepository>();
        services.AddScoped<IVisaVipRepository,VisaVipRepository>();
        services.AddScoped<IAirportServices,AirportServices>();
        


        services.AddScoped<IDtoTranslationService, DtoTranslationService>();

        // Register the seeders
        services.AddScoped<RideBillingTypesConfigSeeder>();
        services.AddHostedService<NotificationSchedulerService>();


        //Mappers
        services.AddAutoMapper(
            typeof(UserMapper).Assembly,
            typeof(WarehouseMapper).Assembly,
            typeof(VehicleMapper).Assembly,
            typeof(RideMapper).Assembly,
            typeof(DiscountMapper).Assembly,
            typeof(FavouritePlaceMapper).Assembly,
            typeof(RideBillingTypesConfigMapper).Assembly,
            typeof(SupportTicketsMapper).Assembly,
            typeof(DesignatedPlacesMapper).Assembly,
            typeof(SettingsMapper).Assembly,
            typeof(ReviewMapper).Assembly,
            typeof(RentalMapper).Assembly,
            typeof(TransactionMapper).Assembly,
            typeof(UserGroupsMapper).Assembly,
            typeof(NotificationTemplateMapper).Assembly,
            typeof(UserNotificationMapper).Assembly,
            typeof(WalletMapper).Assembly,
            typeof(AirportServicesMapper).Assembly
        );

        services.AddHttpContextAccessor();

        // Add JWT Authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                };
            });

        // Add Authorization Policies
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy =>
                policy.RequireClaim("role", Roles.Admin.ToString()));

            options.AddPolicy("DriverPolicy", policy =>
                policy.RequireClaim("role", Roles.Driver.ToString()));

            options.AddPolicy("CustomerPolicy", policy =>
                policy.RequireClaim("role", Roles.Customer.ToString()));

            options.AddPolicy("ManagerPolicy", policy =>
                policy.RequireClaim("adminRole", AdministrativeRoles.Manager.ToString()));


            options.AddPolicy("AdminOrManagerPolicy", policy =>
                policy.RequireAssertion(context =>
                    context.User.HasClaim(c => c.Type == "role" && c.Value == Roles.Admin.ToString()) &&
                    context.User.HasClaim(c => c.Type == "adminRole" &&
                                               (c.Value == AdministrativeRoles.Manager.ToString() ||
                                                c.Value == AdministrativeRoles.Administrator.ToString()))));
        });


        return services;
    }
}