using VipProjectV0._1.Db;
using VipTest.AppSettings.models;
using VipTest.Notifications.Models;
using VipTest.RideBillings.Models;
using VipTest.Rides.Utli;
using VipTest.Transactions.models;
using VipTest.Users.Admins;
using VipTest.Users.customers;
using VipTest.Users.Drivers;
using VipTest.Users.Drivers.Models;
using VipTest.Users.Models;
using VipTest.Wallets.Model;

namespace VipTest.RideBillings;

public class RideBillingTypesConfigSeeder
{
    private readonly IRepositoryWrapper _repo;

    public RideBillingTypesConfigSeeder(IRepositoryWrapper rideBillingRepository)
    {
        _repo = rideBillingRepository;
    }

    public async Task SeedAsync()
    {
        var userGroup = await _repo.UserGroupsRepository.Get(x => true);


        var customer = await _repo.CustomerRepository.Get(x => true);
        if (customer == null)
        {
            customer = new Customer
            {
                Username = "customer",
                PhoneNumber = "1234567891",
                Password = BCrypt.Net.BCrypt.HashPassword("string"),
                CustomerStatus = CustomerStatus.Active,
                Role = Roles.Customer,
            };
            var newcustomer = await _repo.CustomerRepository.Add(customer);
            var CustomerWallet = new Wallet()
            {
                Name = "Customer Wallet",
                UserId = newcustomer.Id,
                User = newcustomer,
                Balance = 0,
                TotalIncome = 0,
                TotalExpense = 0,
                Transactions = new List<Transaction>()
            };
            customer.Wallets.Add(CustomerWallet);

            await _repo.WalletRepository.Add(CustomerWallet);
            
            await _repo.CustomerRepository.Update(customer, customer.Id);
        }


        var Driver = await _repo.DriverRepository.Get(x => true);
        if (Driver == null)
        {
            Driver = new Driver
            {
                Username = "driver",
                PhoneNumber = "1234567890",
                Password = BCrypt.Net.BCrypt.HashPassword("string"),
                DriverStatus = DriverStatus.Available,
                Role = Roles.Driver
            };
            await _repo.DriverRepository.Add(Driver);

            var DriverWallet = new Wallet()
            {
                Name = "Driver Wallet",
                UserId = Driver.Id,
                Balance = 0,
                TotalIncome = 0,
                TotalExpense = 0,
                Transactions = new List<Transaction>()
            };

            await _repo.WalletRepository.Add(DriverWallet);

            Driver.Wallets.Add(DriverWallet);

            await _repo.DriverRepository.Update(Driver, Driver.Id);
        }

        var admin = await _repo.AdminRepository.Get(x => true);
        if (admin == null)
        {
            admin = new Admin
            {
                Username = "admin",
                PhoneNumber = "1234567892",
                Password = BCrypt.Net.BCrypt.HashPassword("string"),
                AdministrativeRole = AdministrativeRoles.Manager,
                Role = Roles.Admin
            };
            await _repo.AdminRepository.Add(admin);
            
            var AdminWallet = new Wallet()
            {
                Name = "Admin Wallet",
                UserId = admin.Id,
                Balance = 0,
                TotalIncome = 0,
                TotalExpense = 0,
                Transactions = new List<Transaction>()
            };
            
            await _repo.WalletRepository.Add(AdminWallet);
            admin.Wallets.Add(AdminWallet);
            await _repo.AdminRepository.Update(admin, admin.Id);
        }
        
        
        var mainWallet = await _repo.WalletRepository.Get(x => x.WalletCode == 0);
        if (mainWallet == null)
        {
            mainWallet = new Wallet()
            {
                WalletCode = 0,
                Name = "Main Wallet",
                Balance = 0,
                TotalIncome = 0,
                TotalExpense = 0,
                Transactions = new List<Transaction>()
            };
            await _repo.WalletRepository.Add(mainWallet);
        }


        // Ensure there's a single Settings instance
        var settings = await _repo.SettingsRepository.Get(x => true);
        if (settings == null)
        {
            settings = new Settings
            {
                UrlWebsite = "https://example.com",
                Address = "123 Example Street",
                Email = "info@example.com",
                PhoneWhatsapp = "1234567890",
                IosAppLink = "https://example.com/ios",
                AndroidAppLink = "https://example.com/android",
                UrlFacebook = "https://facebook.com/example",
                UrlLinkedin = "https://linkedin.com/example",
                UrlInstagram = "https://instagram.com/example",
                PrivacyPolicy = "https://example.com/privacy",
                TermsAndConditions = "https://example.com/terms",
                AboutUs = "About us content",
                EmergncyPhone = "911",
                DriverCommission = 25000,
                VisaCommission = 10000,
                MissingBaggageCommission = 10000,
                VipLoungeCommission = 10000,
                WelcomePackageCommission = 10000,
                IqdToUsd = 0.00076m,

                // Add other default settings values if needed
            };

            await _repo.SettingsRepository.Add(settings);
        }

        var rideTypes = Enum.GetValues(typeof(RideType)).Cast<RideType>();

        foreach (var rideType in rideTypes)
        {
            // Check if the RideBillingTypesConfig for this rideType already exists
            var existingBillingConfig = await _repo.RideBillingRepository.Get(x => x.RideType == rideType);
            if (existingBillingConfig == null)
            {
                // Create a default RideBillingTypesConfig for this RideType
                var newRideBillingTypeConfig = new RideBillingTypesConfig
                {
                    RideType = rideType,
                    Name = rideType.ToString(),
                    Description = $"Billing configuration for {rideType.ToString()}",
                    BaseFarePrice = 10000, // Set default values for BaseFarePrice
                    DetourFarePrice = 5000 // Set default values for DetourFarePrice
                };

                // Add the new RideBillingTypesConfig
                await _repo.RideBillingRepository.Add(newRideBillingTypeConfig);

                // Add the new RideBillingTypesConfig to the Settings instance
                settings.RideBillingTypesConfigs.Add(newRideBillingTypeConfig);
            }
        }

        if (userGroup == null)
        {
            userGroup = new UserGroups
            {
                code = "0",
                Title = "Admin",
                Description = "Admin Group",
                userIds = new List<Guid>()
            };


            var customerGroup = new UserGroups
            {
                code = "1",
                Title = "Customer",
                Description = "Customer Group",
                userIds = new List<Guid>()
            };

            var driverGroup = new UserGroups
            {
                code = "2",
                Title = "Driver",
                Description = "Driver Group",
                userIds = new List<Guid>()
            };

            var allAdmins = await _repo.AdminRepository.getAllAdmins();
            var allDrivers = await _repo.DriverRepository.GetAllDrivers();
            var allCustomers = await _repo.CustomerRepository.GetAllCustomers();

            foreach (var variableAdmin in allAdmins)
            {
                userGroup.userIds?.Add(variableAdmin.Id);
            }

            foreach (var variableDriver in allDrivers)
            {
                driverGroup.userIds?.Add(variableDriver.Id);
            }

            foreach (var variableCustomer in allCustomers)
            {
                customerGroup.userIds?.Add(variableCustomer.Id);
            }

            await _repo.UserGroupsRepository.Add(userGroup);
            await _repo.UserGroupsRepository.Add(driverGroup);
            await _repo.UserGroupsRepository.Add(customerGroup);
        }


        // Save the changes to the Settings entity
        await _repo.SettingsRepository.Update(settings, settings.Id);
    }
}