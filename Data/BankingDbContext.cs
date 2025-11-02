using Microsoft.EntityFrameworkCore;
using Banking_CapStone.Model;
using Banking_CapStone.Service;

namespace Banking_CapStone.Data
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserBase> Users { get; set; }
        public DbSet<SuperAdmin> SuperAdmins { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankUser> BankUsers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountStatus> AccountStatuses { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<PaymentStatus> PaymentStatuses { get; set; }
        public DbSet<ProofType> ProofTypes { get; set; }
        public DbSet<Query> Queries { get; set; }
        public DbSet<SalaryDisbursement> SalaryDisbursements { get; set; }
        public DbSet<SalaryDisbursementDetails> SalaryDisbursementDetails { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === Discriminator for inheritance ===
            modelBuilder.Entity<UserBase>()
                .HasDiscriminator<string>("UserType")
                .HasValue<SuperAdmin>("SuperAdmin")
                .HasValue<BankUser>("BankUser")
                .HasValue<Client>("Client");

            // === Relationships ===
            modelBuilder.Entity<UserBase>()
                .HasOne(u => u.UserRole)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SuperAdmin>()
                .HasMany(sa => sa.Banks)
                .WithOne(b => b.SuperAdmin)
                .HasForeignKey(b => b.SuperAdminId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bank>()
                .HasMany(b => b.BankUsers)
                .WithOne(bu => bu.Bank)
                .HasForeignKey(bu => bu.BankId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bank>()
                .HasMany(b => b.Clients)
                .WithOne(c => c.Bank)
                .HasForeignKey(c => c.BankId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Accounts)
                .WithOne(a => a.Client)
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bank>()
                .HasMany(b => b.Accounts)
                .WithOne(a => a.Bank)
                .HasForeignKey(a => a.BankId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.AccountStatus)
                .WithMany(c => c.Accounts)
                .HasForeignKey(a => a.AccountStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Employees)
                .WithOne(e => e.Client)
                .HasForeignKey(e => e.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Beneficiaries)
                .WithOne(b => b.Client)
                .HasForeignKey(b => b.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // === Constraints ===
            modelBuilder.Entity<UserBase>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<UserBase>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<Account>().HasIndex(a => a.AccountNumber).IsUnique();
            modelBuilder.Entity<Bank>().HasIndex(b => b.IFSCCode).IsUnique();

            // === Defaults ===
            modelBuilder.Entity<Client>().Property(c => c.AccountBalance).HasDefaultValue(0).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Account>().Property(a => a.Balance).HasDefaultValue(0).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Employee>().Property(e => e.Salary).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Payment>().Property(p => p.Amount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Transaction>().Property(t => t.Amount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<SalaryDisbursement>().Property(sd => sd.TotalAmount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<SalaryDisbursementDetails>().Property(sdd => sdd.Amount).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<UserBase>().Property(u => u.IsActive).HasDefaultValue(true);
            modelBuilder.Entity<Employee>().Property(e => e.IsActive).HasDefaultValue(true);
            modelBuilder.Entity<Beneficiary>().Property(b => b.IsActive).HasDefaultValue(true);
            modelBuilder.Entity<Query>().Property(q => q.IsResolved).HasDefaultValue(false);

            // === Seed Data ===
            var passwordHasher = new PasswordHasher();
            string superAdminPass = passwordHasher.HashPassword("Admin@123");
            string bankUserPass = passwordHasher.HashPassword("Bank@123");
            string clientPass = passwordHasher.HashPassword("Client@123");

            // Roles
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { RoleId = 1, Role = Role.SUPER_ADMIN, Description = "Super Administrator" },
                new UserRole { RoleId = 2, Role = Role.BANK_USER, Description = "Bank Employee" },
                new UserRole { RoleId = 3, Role = Role.CLIENT_USER, Description = "Corporate Client" }
            );

            // SuperAdmins
            modelBuilder.Entity<SuperAdmin>().HasData(
                new SuperAdmin { Id = 1, Username = "superadmin", Email = "admin@bankingsystem.com", PasswordHash = superAdminPass, FullName = "System Administrator", RoleId = 1, IsActive = true, CreatedAt = DateTime.UtcNow }
            );

            // Banks
            modelBuilder.Entity<Bank>().HasData(
                new Bank { BankId = 1, BankName = "State Bank of India", IFSCCode = "SBIN0001234", Address = "Mumbai", ContactNumber = "1800112233", SupportEmail = "support@sbi.com", SuperAdminId = 1 },
                new Bank { BankId = 2, BankName = "HDFC Bank", IFSCCode = "HDFC0001234", Address = "Delhi", ContactNumber = "1800202020", SupportEmail = "support@hdfc.com", SuperAdminId = 1 },
                new Bank { BankId = 3, BankName = "ICICI Bank", IFSCCode = "ICIC0001234", Address = "Bangalore", ContactNumber = "1800303030", SupportEmail = "support@icici.com", SuperAdminId = 1 },
                new Bank { BankId = 4, BankName = "Axis Bank", IFSCCode = "AXIS0001234", Address = "Chennai", ContactNumber = "1800404040", SupportEmail = "support@axis.com", SuperAdminId = 1 }
            );

            modelBuilder.Entity<BankUser>().HasData(
    new BankUser
    {
        Id = 5,
        Username = "bankuser1",
        Email = "bankuser1@globalbank.com",
        PasswordHash = bankUserPass,
        FullName = "John Global",
        RoleId = 2,
        BankId = 1,
        IsActive = true,
        CreatedAt = DateTime.UtcNow
    },
    new BankUser
    {
        Id = 6,
        Username = "bankuser2",
        Email = "bankuser2@citybank.com",
        PasswordHash = bankUserPass,
        FullName = "Ravi City",
        RoleId = 2,
        BankId = 2,
        IsActive = true,
        CreatedAt = DateTime.UtcNow
    },
    new BankUser
    {
        Id = 7,
        Username = "bankuser3",
        Email = "bankuser3@metrobank.com",
        PasswordHash = bankUserPass,
        FullName = "Priya Metro",
        RoleId = 2,
        BankId = 3,
        IsActive = true,
        CreatedAt = DateTime.UtcNow
    },
    new BankUser
    {
        Id = 8,
        Username = "bankuser4",
        Email = "bankuser4@coastalbank.com",
        PasswordHash = bankUserPass,
        FullName = "Kiran Coastal",
        RoleId = 2,
        BankId = 4,
        IsActive = true,
        CreatedAt = DateTime.UtcNow
    }
);

            // Clients
            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 6, Username = "techcorp", Email = "admin@techcorp.com", PasswordHash = clientPass, ClientName = "TechCorp Solutions Pvt Ltd", AccountNumber = "ACC001", AccountBalance = 5000000, BankId = 1, RoleId = 3, IsActive = true },
                new Client { Id = 7, Username = "globaltraders", Email = "admin@globaltraders.com", PasswordHash = clientPass, ClientName = "Global Traders Ltd", AccountNumber = "ACC002", AccountBalance = 7500000, BankId = 2, RoleId = 3, IsActive = true },
                new Client { Id = 8, Username = "manufacturing", Email = "admin@manufacturing.com", PasswordHash = clientPass, ClientName = "Manufacturing Industries Inc", AccountNumber = "ACC003", AccountBalance = 10000000, BankId = 3, RoleId = 3, IsActive = true },
                new Client { Id = 9, Username = "finserve", Email = "admin@finserve.com", PasswordHash = clientPass, ClientName = "FinServe Pvt Ltd", AccountNumber = "ACC004", AccountBalance = 6000000, BankId = 4, RoleId = 3, IsActive = true }
            );

            // Employees
            modelBuilder.Entity<Employee>().HasData(
                new Employee { EmployeeId = 1, FullName = "Suresh Patil", Email = "suresh@techcorp.com", Salary = 75000, ClientId = 6, IsActive = true },
                new Employee { EmployeeId = 2, FullName = "Kavita Desai", Email = "kavita@techcorp.com", Salary = 65000, ClientId = 6, IsActive = true },
                new Employee { EmployeeId = 3, FullName = "Ramesh Joshi", Email = "ramesh@globaltraders.com", Salary = 80000, ClientId = 7, IsActive = true },
                new Employee { EmployeeId = 4, FullName = "Anjali Mehta", Email = "anjali@manufacturing.com", Salary = 90000, ClientId = 8, IsActive = true }
            );
        }
    }
}