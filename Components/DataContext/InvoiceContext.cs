using InvoiceWebApp.Components.Entities;
using Microsoft.EntityFrameworkCore;


namespace InvoiceWebApp.Components.DataContext
{
    public partial class InvoiceContext : DbContext
    {
        public InvoiceContext(DbContextOptions<InvoiceContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) { }
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Debtor> Debtors { get; set; }
        public virtual DbSet<DebtorHasAddress> DebtorHasAddresses { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => new { e.Number, e.PostalCode });

                entity.ToTable("addresses");

                entity.Property(e => e.Number).HasColumnType("int");
                entity.Property(e => e.PostalCode).HasColumnType("VARCHAR(40)");

                entity.Property(e => e.City).IsRequired()
                       .HasColumnType("VARCHAR(150)");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnType("VARCHAR(150)");
                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasColumnType("VARCHAR(150)");

                entity.Property(e => e.Suffix)
                    .HasColumnType("VARCHAR(10)");
            });

            modelBuilder.Entity<Debtor>(entity =>
            {
                entity.ToTable("debtors");

                entity.HasIndex(e => e.Id)
                      .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("VARCHAR(200)");

                entity.Property(e => e.BankAccount)
                    .IsRequired()
                    .HasColumnType("VARCHAR(50)");

                entity.Property(e => e.CompanyName)
                    .HasColumnType("VARCHAR(250)"); ;

                entity.Property(e => e.Email)
                    .IsRequired()
                   .HasColumnType("VARCHAR(100)");

                entity.Property(e => e.FirstName)
                   .HasColumnType("VARCHAR(100)");

                entity.Property(e => e.LastName)
                    .HasColumnType("VARCHAR(100)");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnType("VARCHAR(40)");
            });

            modelBuilder.Entity<DebtorHasAddress>(entity =>
            {
                entity.HasKey(e => new { e.DebtorId, e.PostalCode, e.Number });

                entity.ToTable("debtor_has_addresses");

                entity.Property(e => e.DebtorId)
                    .HasColumnType("VARCHAR(200)");

                entity.HasOne(e => e.Debtor)
                    .WithMany(e => e.Addresses)
                    .HasConstraintName("dha_debtor_fk")
                    .HasForeignKey(e => e.DebtorId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Address)
                    .WithMany(e => e.Debtors)
                    .HasConstraintName("dha_address_fk")
                    .HasForeignKey(e => new { e.Number, e.PostalCode })
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.PostalCode)
                   .HasColumnType("VARCHAR(40)");

                entity.Property(e => e.Number)
                    .HasColumnType("INT");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.InvoiceNumber);

                entity.ToTable("invoices");

                entity.Property(e => e.InvoiceNumber)
                    .HasColumnType("VARCHAR(200)");

                entity.Property(e => e.Comment)
                    .HasColumnType("VARCHAR(250)");

                entity.Property(e => e.CreatedOn)
                      .HasColumnType("datetime");

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasColumnType("VARCHAR(200)");

                entity.HasOne(e => e.Debtor)
                    .WithMany(e => e.Invoices)
                    .HasForeignKey(e => e.CustomerId)
                    .HasConstraintName("invoices_debtors_fk");

                entity.Property(e => e.ExpiredOn)
                      .HasColumnType("datetime");

                entity.Property(e => e.IsPaid)
                     .HasColumnType("bit");

                entity.Property(e => e.Concept)
                     .HasColumnType("bit");

                entity.Property(e => e.Discount).HasColumnName("discount").HasColumnType("INT");

                entity.Property(e => e.Total).HasColumnName("total").HasColumnType("decimal");
            });

            modelBuilder.Entity<InvoiceItem>(entity =>
            {
                entity.HasKey(e => new { e.ItemNumber, e.InvoiceNumber });

                entity.ToTable("invoice_items");

                entity.Property(e => e.ItemNumber)
                     .HasColumnType("INT")
                    .ValueGeneratedOnAdd();

                entity.HasOne(e => e.Invoice)
                    .WithMany(e => e.Items)
                    .HasForeignKey(e => e.InvoiceNumber)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("items_invoice_fk");

                entity.Property(e => e.InvoiceNumber)
                    .HasColumnType("VARCHAR(200)");

                entity.Property(e => e.Description)
                   .HasColumnType("VARCHAR(250)");

                entity.Property(e => e.Name)
                    .IsRequired()
                   .HasColumnType("VARCHAR(200)");

                entity.Property(e => e.Price).HasColumnType("DECIMAL");

                entity.Property(e => e.Quantity)
                      .HasColumnType("int");

                entity.Property(e => e.Tax)
                      .HasColumnType("int");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("roles");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Type)
                    .HasColumnType("VARCHAR(200)");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Email);

                entity.ToTable("users");

                entity.Property(e => e.Email)
                    .HasColumnType("VARCHAR(250)");

                entity.Property(e => e.FirstName)
                    .HasColumnType("VARCHAR(150)");

                entity.Property(e => e.LastName)
                    .HasColumnType("VARCHAR(175)");

                entity.Property(e => e.CompanyName)
                    .HasColumnType("VARCHAR(250)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("VARCHAR(255)");

                entity.Property(e => e.Picture)
                    .HasColumnName("picture");

                entity.Property(e => e.Role)
                      .HasColumnType("int");
            });

            modelBuilder.Entity<Settings>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("settings");

                entity.Property(e => e.Id)
                      .HasColumnType("int");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasColumnType("VARCHAR(300)");

                entity.Property(e => e.Website)
                    .HasColumnType("VARCHAR(300)");

                entity.Property(e => e.Phone)
                    .HasColumnType("VARCHAR(40)"); ;

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("VARCHAR(150)");

                entity.Property(e => e.Password)
                    .IsRequired()
                   .HasColumnType("VARCHAR(80)");

                entity.Property(e => e.SMTP)
                    .IsRequired()
                    .HasColumnType("VARCHAR(80)");

                entity.Property(e => e.Port)
                .IsRequired()
                     .HasColumnType("int");

                entity.Property(e => e.BankAccount)
                    .IsRequired()
                    .HasColumnType("VARCHAR(150)");

                entity.Property(e => e.Bank)
                    .IsRequired()
                    .HasColumnType("VARCHAR(150)");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnType("VARCHAR(200)");

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                  .HasColumnType("VARCHAR(20)");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnType("VARCHAR(250)");

                entity.Property(e => e.Country)
                    .IsRequired()
                   .HasColumnType("VARCHAR(250)");

                entity.Property(e => e.BusinessNumber)
                    .IsRequired()
                   .HasColumnType("VARCHAR(250)");

                entity.Property(e => e.VAT)
                    .IsRequired()
                  .HasColumnType("VARCHAR(250)");

                entity.Property(e => e.InvoicePrefix)
                   .HasColumnType("VARCHAR(20)");

                entity.Property(e => e.Logo)
                    .HasColumnName("logo");

                entity.Property(e => e.Color)
                    .HasColumnName("color");

                entity.Property(e => e.ShowLogo)
                    .IsRequired()
                    .HasColumnType("bit");

                entity.Property(e => e.ShowLogoInPDF)
                    .IsRequired()
                   .HasColumnType("bit");
            });
        }
    }
}  


