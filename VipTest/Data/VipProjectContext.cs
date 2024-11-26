using Microsoft.EntityFrameworkCore;
using VipTest.AirPortServices.models;
using VipTest.AppSettings.models;
using VipTest.attachmentsConfig;
using VipTest.Discounts.Models;
using VipTest.FavPlaces.models;
using VipTest.Files.Models;
using VipTest.Notifications.Models;
using VipTest.Rentals.Models;
using VipTest.reviews.models;
using VipTest.RideBillings.Models;
using VipTest.Rides.Models;
using VipTest.Tickets.models;
using VipTest.Transactions.models;
using VipTest.Users.Admins;
using VipTest.Users.BranchManagers;
using VipTest.Users.customers;
using VipTest.Users.Drivers.Models;
using VipTest.Users.Models;
using VipTest.Users.OTP;
using VipTest.Utlity;
using VipTest.Utlity.Basic;
using VipTest.vehicles.Modles;
using VipTest.Wallets.Model;
using VipTest.Warehouses.Models;

namespace VipTest.Data;

public class VipProjectContext : DbContext
{
    public VipProjectContext(DbContextOptions<VipProjectContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<BranchManager> BranchManagers { get; set; }
    public DbSet<ProjectFiles> Files { get; set; } // DbSet for the File entity
    public DbSet<Vehicles> Vehicles { get; set; }
    public DbSet<Ride> Rides { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<FavouritePlace> FavouritePlaces { get; set; }
    public DbSet<RideBillingTypesConfig> RideBillings { get; set; } // DbSet for the RideBillingTypesConfig entity
    public DbSet<Discount> Discounts { get; set; } // DbSet for the Discount entity
    public DbSet<Attachments> Attachments { get; set; }
    public DbSet<SupportTickets> SupportTickets { get; set; }
    public DbSet<Settings> Settings { get; set; }
    public DbSet<DiscountUsage> DiscountUsages { get; set; } // DbSet for the DiscountUsage entity 
    public DbSet<Review> Reviews { get; set; } // DbSet for the Review entity
    public DbSet<DriverReview> DriverReviews { get; set; } // DbSet for the DriverReview entity
    public DbSet<RideReview> RideReviews { get; set; } // DbSet for the RideReview entity
    public DbSet<VehicleReview> VehicleReviews { get; set; } // DbSet for the VehicleReview entity
    public DbSet<CarRentalOrder> CarRentalOrders { get; set; } // DbSet for the CarRentalOrder entity
    public DbSet<Transaction> Transactions { get; set; } // DbSet for the Transaction entity
    public DbSet<UserNotification> Notifications { get; set; } // DbSet for the Notification entity
    public DbSet<NotificationTemplate> NotificationTemplates { get; set; } // DbSet for the NotificationTemplate entity
    public DbSet<UserGroups> UserGroups { get; set; } // DbSet for the UserGroups entity
    public DbSet<Wallet> Wallets { get; set; } // DbSet for the Wallet entity
    public DbSet<AirportServicesModel> AirportServices { get; set; } // DbSet for the AirportServicesModel entity
    public DbSet<VisaVipService> VisaVipServices { get; set; } // DbSet for the VisaVipService entity
    public DbSet<LoungeService> LoungeServices { get; set; } // DbSet for the LoungeService entity
    public DbSet<LuggageService> LuggageServices { get; set; } // DbSet for the LuggageService entity
    
    
    public DbSet<PendingCustomer?> OtpCustomers { get; set; } // DbSet for the PendingCustomer entity

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define one-to-one relationship between Warehouse and BranchManager
        modelBuilder.Entity<Warehouse>()
            .HasOne(w => w.BranchManager)
            .WithOne(bm => bm.Warehouse)
            .HasForeignKey<Warehouse>(w => w.BranchManagerId); // Warehouse has the foreign key

        
        modelBuilder.Entity<Driver>().HasBaseType<User>();
        modelBuilder.Entity<Customer>().HasBaseType<User>();
        modelBuilder.Entity<Admin>().HasBaseType<User>();
        modelBuilder.Entity<BranchManager>().HasBaseType<User>();

        modelBuilder.Entity<DriverReview>().HasBaseType<Review>();
        modelBuilder.Entity<RideReview>().HasBaseType<Review>();
        modelBuilder.Entity<VehicleReview>().HasBaseType<Review>();
        
        modelBuilder.Entity<LuggageService>().HasBaseType<AirportServicesModel>();
        modelBuilder.Entity<LoungeService>().HasBaseType<AirportServicesModel>();
        modelBuilder.Entity<VisaVipService>().HasBaseType<AirportServicesModel>();

        modelBuilder.Entity<FavouritePlace>()
            .HasOne(fp => fp.Customer)
            .WithMany(c => c.FavoritePlaces)
            .HasForeignKey(fp => fp.CustomerId);

        modelBuilder.Entity<Discount>()
            .Property(d => d.StartDate)
            .HasColumnType("timestamp with time zone");

        modelBuilder.Entity<Discount>()
            .Property(d => d.EndDate)
            .HasColumnType("timestamp with time zone");

        modelBuilder.Entity<DiscountUsage>()
            .HasKey(du => du.Id);

        modelBuilder.Entity<DiscountUsage>()
            .HasOne(du => du.Discount)
            .WithMany(d => d.UserUsageCounts)
            .HasForeignKey(du => du.DiscountId);

        // Configure the relationships between Transaction and Wallet
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.FromWallet)
            .WithMany(w => w.Transactions) // Assuming each wallet can have multiple transactions
            .HasForeignKey(t => t.FromWalletId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.ToWallet)
            .WithMany()
            .HasForeignKey(t => t.ToWalletId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

        base.OnModelCreating(modelBuilder);
    }


    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity<Guid> &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            var entity = (BaseEntity<Guid>)entityEntry.Entity;
            entity.UpdatedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
        }
    }
}