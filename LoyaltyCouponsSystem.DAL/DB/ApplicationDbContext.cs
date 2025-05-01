using LoyaltyCouponsSystem.DAL.Entity;
using LoyaltyCouponsSystem.DAL.Entity.Permission;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoyaltyCouponsSystem.DAL.DB
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<GlobalCounter> GlobalCounters { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<StoreKeeper> StoreKeepers { get; set; }

        public DbSet<Technician> Technicians { get; set; }
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Representative> Representatives { get; set; }
        public DbSet<TechnicianUser> TechnicianUsers { get; set; }
        public DbSet<TechnicianCustomer> TechnicianCustomers { get; set; }

        public DbSet<Governorate> Governorates { get; set; }
        
        public DbSet<Area> Areas { get; set; }

        public DbSet<QRScanLog> QRScanLogs { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }

        public DbSet<QRCodeTransactionGenerated> qRCodeTransactionGenerateds { get; set; }
        public DbSet<TransactionForRecieptFromRepToCust> TransactionForRecieptFromRepToCusts { get; set; }


        public DbSet<DeliverFromRepToCoust> DeliverFromRepToCousts { get; set; }
        public DbSet<ReceiveFromCustomer> ReceiveFromCustomers { get; set; }
        public DbSet<ApproveRecievedCoupons> ApproveRecievedCoupons { get; set; }
        public DbSet<LogIssue> LogIssue { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Always call base first for Identity configuration

            // Ensure primary key for IdentityUserLogin<string>
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(l => new { l.LoginProvider, l.ProviderKey });
            });

            // Ensure QRCode serialnumber is unique
            modelBuilder.Entity<Coupon>()
            .HasIndex(c => c.SerialNumber)
            .IsUnique();

            // Ensure Code is unique
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Code)
                .IsUnique();

            // Ensure PhoneNumber is unique
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.PhoneNumber)
                .IsUnique();

            // Ensure Code is unique
            modelBuilder.Entity<Technician>()
                .HasIndex(c => c.Code)
                .IsUnique();

            // Ensure PhoneNumber is unique
            modelBuilder.Entity<Technician>()
                .HasIndex(c => c.PhoneNumber1)
                .IsUnique();
            // Ensure Code is unique
            modelBuilder.Entity<Distributor>()
                .HasIndex(c => c.Code)
                .IsUnique();

            // Ensure PhoneNumber is unique
            modelBuilder.Entity<Distributor>()
                .HasIndex(c => c.PhoneNumber1)
                .IsUnique();

            modelBuilder.Entity<DistributorCustomer>()
    .HasKey(dc => new { dc.DistributorID, dc.CustomerID });

            modelBuilder.Entity<DistributorCustomer>()
                .HasOne(dc => dc.Distributor)
                .WithMany(d => d.DistributorCustomers)
                .HasForeignKey(dc => dc.DistributorID)
                .OnDelete(DeleteBehavior.Cascade); // Ensures cascading deletes if a Distributor is removed

            modelBuilder.Entity<DistributorCustomer>()
                .HasOne(dc => dc.Customer)
                .WithMany(c => c.DistributorCustomers)
                .HasForeignKey(dc => dc.CustomerID)
                .OnDelete(DeleteBehavior.Restrict); // Prevents accidental customer deletion


            //Governate and Area
            modelBuilder.Entity<Governorate>()
                .HasMany(g => g.Areas)
                .WithOne(a => a.Governorate)
                .HasForeignKey(a => a.GovernateId)
                .OnDelete(DeleteBehavior.Cascade); // عند حذف المحافظة يتم حذف المناطق

            // Ensure NationalID is unique
            modelBuilder.Entity<ApplicationUser>()
                    .HasIndex(u => u.NationalID)
                    .IsUnique();

            // Ensure PhoneNumber is unique
            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<Admin>()
                .HasOne(d => d.ApplicationUser)
                .WithOne(au => au.Admin)
                .HasForeignKey<Admin>(d => d.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Representative>()
                .HasOne(d => d.ApplicationUser)
                .WithOne(au => au.Representative)
                .HasForeignKey<Representative>(d => d.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerID);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Governate)
                    .HasMaxLength(50);

                entity.Property(e => e.City)
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(11);
            });

            // Configure the one-to-one relationship between Coupon and QRScanLog
            modelBuilder.Entity<Coupon>()
            .HasOne(c => c.QRScanLog)
            .WithOne(q => q.Coupon)
            .HasForeignKey<QRScanLog>(q => q.CouponId)  // Ensure correct foreign key
            .OnDelete(DeleteBehavior.Cascade); // Delete QRScanLog if Coupon is deleted


            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionID);

                // Ensure unique sequences within the same ExchangePermission
                // entity.HasIndex(t => new { t.ExchangePermission, t.SequenceStart, t.SequenceEnd }).IsUnique();

                entity.Property(e => e.TransactionType)
                    .HasMaxLength(50);

                entity.Property(e => e.PurchaseAmount)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.Transactions)
                    .HasForeignKey(e => e.CustomerID);
                entity.HasOne(e => e.Distributor)
                    .WithMany(c => c.Transactions)
                    .HasForeignKey(e => e.DistributorID);
            });



            modelBuilder.Entity<GlobalCounter>(entity =>
            {
                entity.HasKey(GC => GC.Year);
            });


            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.HasKey(e => e.CouponeId);

                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<StoreKeeper>(entity =>
            {
                entity.HasKey(e => e.StoreKeeperID);
                entity.Property(e => e.NameAttribute).IsRequired().HasMaxLength(100);
            });

            // Technician-Customer many-to-many
            modelBuilder.Entity<TechnicianCustomer>()
                .HasKey(tc => new { tc.TechnicianId, tc.CustomerId });

            modelBuilder.Entity<TechnicianCustomer>()
                .HasOne(tc => tc.Technician)
                .WithMany(t => t.TechnicianCustomers)
                .HasForeignKey(tc => tc.TechnicianId);

            modelBuilder.Entity<TechnicianCustomer>()
                .HasOne(tc => tc.Customer)
                .WithMany(c => c.TechnicianCustomers)
                .HasForeignKey(tc => tc.CustomerId);

            // Technician-User many-to-many
            modelBuilder.Entity<TechnicianUser>()
                .HasKey(tu => new { tu.TechnicianId, tu.UserId });

            modelBuilder.Entity<TechnicianUser>()
                .HasOne(tu => tu.Technician)
                .WithMany(t => t.TechnicianUsers)
                .HasForeignKey(tu => tu.TechnicianId);

            modelBuilder.Entity<TechnicianUser>()
                .HasOne(tu => tu.User)
                .WithMany(u => u.TechnicianUsers)
                .HasForeignKey(tu => tu.UserId);




            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeID);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.AuditLogID);
                entity.Property(e => e.Action).IsRequired().HasMaxLength(200);
            });


            modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany()  // IdentityRole has no RolePermissions collection, so we use WithMany() without a navigation property
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)  // This assumes you add the RolePermissions navigation property in Permission
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserPermission>()
            .HasKey(rp => new { rp.UserId, rp.PermissionId });

            modelBuilder.Entity<UserPermission>()
                .HasOne(rp => rp.User)
                .WithMany()  // IdentityRole has no RolePermissions collection, so we use WithMany() without a navigation property
                .HasForeignKey(rp => rp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserPermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.UserPermissions)  // This assumes you add the RolePermissions navigation property in Permission
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed initial permissions to the Permissions table
            modelBuilder.Entity<Permission>().HasData(
                new Permission { Id = 1, Name = "Manage Customers" },               
                new Permission { Id = 2, Name = "Deliver From Representative to Customer" },               
                new Permission { Id = 3, Name = "Approve Recieved Coupons" },               
                new Permission { Id = 4, Name = "Manage Users" },
                new Permission { Id = 5, Name = "Generate QR Codes" },
                new Permission { Id = 6, Name = "Exchange Permissions" },
                new Permission { Id = 7, Name = "Receive From Customer" }            
            );

        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is ApplicationUser && e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                entry.State = EntityState.Modified;
                ((ApplicationUser)entry.Entity).IsDeleted = true;
            }

            return base.SaveChanges();
        }
    }
}
