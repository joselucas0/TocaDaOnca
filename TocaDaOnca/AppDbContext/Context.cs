using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TocaDaOnca.Models;

namespace TocaDaOnca.AppDbContext
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Kiosk> Kiosks { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleProduct> SalesProducts { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReservationVisitor> ReservationVisitors { get; set; }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // // Configurações do banco de dados
        // {
        //     var config = new ConfigurationBuilder()
        //         .SetBasePath(Directory.GetCurrentDirectory())
        //         .AddJsonFile("appsettings.json")
        //         .Build();

        //     optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));
        // }

        public Context(DbContextOptions<Context> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region USER

            modelBuilder.Entity<User>()
                .Property(u => u.Cpf)
                .HasColumnType("char(15)");

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasColumnType("timestamp with time zone")  // TODO: Create trigger or function before saveChanges() to set this value on update
                .HasDefaultValueSql("now()");

            #endregion

            #region KIOSK

            modelBuilder.Entity<Kiosk>()
                .Property(k => k.Description)
                .HasColumnType("text");

            modelBuilder.Entity<Kiosk>()
                .Property(k => k.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Kiosk>()
                .Property(k => k.UpdatedAt)
                .HasColumnType("timestamp with time zone")  // TODO: Create trigger or function before saveChanges() to set this value on update
                .HasDefaultValueSql("now()");

            #endregion

            #region PRODUCT

            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .HasColumnType("text");

            modelBuilder.Entity<Product>()
                .Property(p => p.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Product>()
                .Property(p => p.UpdatedAt)
                .HasColumnType("timestamp with time zone")  // TODO: Create trigger or function before saveChanges() to set this value on update
                .HasDefaultValueSql("now()");

            #endregion

            #region EMPLOYEE

            modelBuilder.Entity<Employee>()
                .Property(e => e.Cpf)
                .HasColumnType("char(15)");

            modelBuilder.Entity<Employee>()
                .Property(e => e.Manager)
                .HasDefaultValueSql("false");

            modelBuilder.Entity<Employee>()
                .Property(e => e.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Employee>()
                .Property(e => e.UpdatedAt)
                .HasColumnType("timestamp with time zone")  // TODO: Create trigger or function before saveChanges() to set this value on update
                .HasDefaultValueSql("now()");

            #endregion

            #region VISITOR

            modelBuilder.Entity<Visitor>()
                .Property(v => v.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Visitor>()
                .Property(v => v.UpdatedAt)
                .HasColumnType("timestamp with time zone")  // TODO: Create trigger or function before saveChanges() to set this value on update
                .HasDefaultValueSql("now()");

            #endregion

            #region SALE

            modelBuilder.Entity<Sale>()
                .Property(s => s.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Sale>()
                .Property(s => s.UpdatedAt)
                .HasColumnType("timestamp with time zone")  // TODO: Create trigger or function before saveChanges() to set this value on update
                .HasDefaultValueSql("now()");

            #endregion

            #region SALE_PRODUCT

            modelBuilder.Entity<SaleProduct>()
                .HasOne(sp => sp.Sale)
                .WithMany(s => s.SaleProducts) // ← você precisa adicionar essa propriedade em Sale
                .HasForeignKey(sp => sp.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SaleProduct>()
                .HasOne(sp => sp.Product)
                .WithMany(p => p.SaleProducts) // ← você precisa adicionar essa propriedade em Product
                .HasForeignKey(sp => sp.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SaleProduct>()
                .Property(sp => sp.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<SaleProduct>()
                .Property(sp => sp.UpdatedAt)
                .HasColumnType("timestamp with time zone")  // TODO: Create trigger or function before saveChanges() to set this value on update
                .HasDefaultValueSql("now()");

            #endregion

            #region REPORT

            modelBuilder.Entity<Report>()
                .ToTable(t => t.HasCheckConstraint("CK_Report_ReportType", "report_type IN ('S', 'M', 'A')"))   // S: Semanal; M: Mensal; A: Anual
                .Property(r => r.ReportType)
                .HasColumnType("char(1)")
                .HasDefaultValueSql("'S'");


            modelBuilder.Entity<Report>()
                .Property(r => r.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Report>()
                .Property(r => r.UpdatedAt)
                .HasColumnType("timestamp with time zone")  // TODO: Create trigger or function before saveChanges() to set this value on update
                .HasDefaultValueSql("now()");

            #endregion

            #region RESERVATION

            modelBuilder.Entity<Reservation>()
                .Property(r => r.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Reservation>()
                .Property(r => r.UpdatedAt)
                .HasColumnType("timestamp with time zone")  // TODO: Create trigger or function before saveChanges() to set this value on update
                .HasDefaultValueSql("now()");
                
            #endregion

            #region RESERVATION_VISITOR

            modelBuilder.Entity<ReservationVisitor>()
                .Property(rv => rv.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<ReservationVisitor>()
                .Property(rv => rv.UpdatedAt)
                .HasColumnType("timestamp with time zone")  // TODO: Create trigger or function before saveChanges() to set this value on update
                .HasDefaultValueSql("now()");

            #endregion
        }
    }
}