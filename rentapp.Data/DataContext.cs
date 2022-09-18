using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using rentapp.BL.Entities;

namespace rentapp.Data
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Charget> Chargets { get; set; } = null!;
        public virtual DbSet<Condition> Conditions { get; set; } = null!;
        public virtual DbSet<Contract> Contracts { get; set; } = null!;
        public virtual DbSet<ContractStatus> ContractStatuses { get; set; } = null!;
        public virtual DbSet<ContractType> ContractTypes { get; set; } = null!;
        public virtual DbSet<Currency> Currencies { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<CustomerPayment> CustomerPayments { get; set; } = null!;
        public virtual DbSet<CustomerPaymentCard> CustomerPaymentCards { get; set; } = null!;
        public virtual DbSet<CustomerPaymentCash> CustomerPaymentCashes { get; set; } = null!;
        public virtual DbSet<CustomerPaymentFlow> CustomerPaymentFlows { get; set; } = null!;
        public virtual DbSet<BL.Entities.Directory> Directories { get; set; } = null!;
        public virtual DbSet<DocumentType> DocumentTypes { get; set; } = null!;
        public virtual DbSet<Expense> Expenses { get; set; } = null!;
        public virtual DbSet<Period> Periods { get; set; } = null!;
        public virtual DbSet<PictureProperty> PictureProperties { get; set; } = null!;
        public virtual DbSet<Property> Properties { get; set; } = null!;
        public virtual DbSet<PropertyStatus> PropertyStatuses { get; set; } = null!;
        public virtual DbSet<SubPropertyType> SubPropertyTypes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserPermission> UserPermissions { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;
        public virtual DbSet<UserRoleUserPermission> UserRoleUserPermissions { get; set; } = null!;
        public virtual DbSet<UserUserPermission> UserUserPermissions { get; set; } = null!;
        public virtual DbSet<UserUserRole> UserUserRoles { get; set; } = null!;
        public virtual DbSet<DirectoryCustomer> DirectoryCustomers { get; set; }
        public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Charget>(entity =>
            {
                entity.ToTable("Charget");

                entity.Property(e => e.ChargetDate).HasColumnType("datetime");

                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.Observation)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.Chargets)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Charget_Contract");

                entity.HasOne(d => d.CustomerPayment)
                    .WithMany(p => p.Chargets)
                    .HasForeignKey(d => d.CustomerPaymentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Charget_CustomerPayment");
            });

            modelBuilder.Entity<Condition>(entity =>
            {
                entity.ToTable("Condition");

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.ToTable("Contract");

                entity.Property(e => e.ContractFilePath)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FeesAmount).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.FilePath)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IncreasePercentage).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Observation)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.WarrantyAmount).HasColumnType("decimal(18, 5)");

                entity.HasOne(d => d.ContractType)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.ContractTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contract_ContractType");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contract_Customer");

                entity.HasOne(d => d.Directory)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.DirectoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contract_Directory");

                entity.HasOne(d => d.IsCompletedNavigation)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.IsCompleted)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contract_ContractStatus");

                entity.HasOne(d => d.Period)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.PeriodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contract_Period");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contract_Property");
            });

            modelBuilder.Entity<ContractStatus>(entity =>
            {
                entity.ToTable("ContractStatus");

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ContractType>(entity =>
            {
                entity.ToTable("ContractType");

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.ToTable("Currency");

                entity.Property(e => e.CurrencyId).ValueGeneratedNever();

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Rate).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.Symbol)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<CustomerPayment>(entity =>
            {
                entity.ToTable("CustomerPayment");

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Observation)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Total).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.TotalPaid).HasColumnType("decimal(18, 5)");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.CustomerPayments)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerPayment_Contract");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerPayments)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerPayment_Customer");
            });

            modelBuilder.Entity<CustomerPaymentCard>(entity =>
            {
                entity.ToTable("CustomerPaymentCard");

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.CustomerPayment)
                    .WithMany(p => p.CustomerPaymentCards)
                    .HasForeignKey(d => d.CustomerPaymentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerPaymentCard_CustomerPayment");
            });

            modelBuilder.Entity<CustomerPaymentCash>(entity =>
            {
                entity.ToTable("CustomerPaymentCash");

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.CustomerPayment)
                    .WithMany(p => p.CustomerPaymentCashes)
                    .HasForeignKey(d => d.CustomerPaymentId)
                    .HasConstraintName("FK_CustomerPaymentCash_CustomerPayment");
            });

            modelBuilder.Entity<CustomerPaymentFlow>(entity =>
            {
                entity.ToTable("CustomerPaymentFlow");

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Income).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Outcome).HasColumnType("decimal(18, 5)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerPaymentFlows)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerPaymentFlow_Customer");

                entity.HasOne(d => d.CustomerPayment)
                    .WithMany(p => p.CustomerPaymentFlows)
                    .HasForeignKey(d => d.CustomerPaymentId)
                    .HasConstraintName("FK_CustomerPaymentFlow_CustomerPayment");
            });

            modelBuilder.Entity<BL.Entities.Directory>(entity =>
            {
                entity.ToTable("Directory");

                entity.Property(e => e.AdjacentStreet1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AdjacentStreet2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Floor)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LogoFilePath)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Number)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Observation)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PlainAdjacentStreet1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlainAdjacentStreet2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlainCity)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlainCountry)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlainState)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlainStreet)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlainZipCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Unit)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DocumentType>(entity =>
            {
                entity.ToTable("DocumentType");

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Expense>(entity =>
            {
                entity.ToTable("Expense");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Observation)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Directory)
                    .WithMany(p => p.Expenses)
                    .HasForeignKey(d => d.DirectoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Expense_Directory");
            });

            modelBuilder.Entity<Period>(entity =>
            {
                entity.ToTable("Period");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PictureProperty>(entity =>
            {
                entity.ToTable("PictureProperty");

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.PicturePath)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.Poperty)
                    .WithMany(p => p.PictureProperties)
                    .HasForeignKey(d => d.PopertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PictureProperty_Property");
            });

            modelBuilder.Entity<Property>(entity =>
            {
                entity.ToTable("Property");

                entity.Property(e => e.AdjacentStreet1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AdjacentStreet2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ExpensesPrice).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.Floor)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Number)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PlainAdjacentStreet1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlainAdjacentStreet2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlainCity)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlainCountry)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlainState)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlainStreet)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlainZipCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Unit)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Condition)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.ConditionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Property_Condition");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Property_Currency");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Property_Customer");

                entity.HasOne(d => d.Directory)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.DirectoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Property_Directory");

                entity.HasOne(d => d.PropertyStatus)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.PropertyStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Property_PropertyStatus");

                entity.HasOne(d => d.SubPropertyType)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.SubPropertyTypeId)
                    .HasConstraintName("FK_Property_SubPropertyType");
            });

            modelBuilder.Entity<PropertyStatus>(entity =>
            {
                entity.ToTable("PropertyStatus");

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SubPropertyType>(entity =>
            {
                entity.ToTable("SubPropertyType");

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).ValueGeneratedOnAdd();

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Directory)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.DirectoryId)
                    .HasConstraintName("FK_User_Directory");

                entity.HasOne(d => d.UserNavigation)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Customer");
            });

            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.ToTable("UserPermission");

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserRoleUserPermission>(entity =>
            {
                entity.ToTable("UserRoleUserPermission");

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.UserPermission)
                    .WithMany(p => p.UserRoleUserPermissions)
                    .HasForeignKey(d => d.UserPermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRoleUserPermission_UserPermission");

                entity.HasOne(d => d.UserRole)
                    .WithMany(p => p.UserRoleUserPermissions)
                    .HasForeignKey(d => d.UserRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRoleUserPermission_UserRole");
            });

            modelBuilder.Entity<UserUserPermission>(entity =>
            {
                entity.ToTable("UserUserPermission");

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserUserPermissions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserUserPermission_User");

                entity.HasOne(d => d.UserPermission)
                    .WithMany(p => p.UserUserPermissions)
                    .HasForeignKey(d => d.UserPermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserUserPermission_UserPermission");
            });

            modelBuilder.Entity<UserUserRole>(entity =>
            {
                entity.ToTable("UserUserRole");

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserUserRole_User");

                entity.HasOne(d => d.UserRole)
                    .WithMany(p => p.UserUserRoles)
                    .HasForeignKey(d => d.UserRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserUserRole_UserRole");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
