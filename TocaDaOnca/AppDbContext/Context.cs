using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TocaDaOnca.Models;

namespace TocaDaOnca.AppDbContext
{
    public class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Kiosk> Kiosks { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region USER

            modelBuilder.Entity<User>()
                .ToTable("users", u => u.HasCheckConstraint("ck_user_plan_valid_values", "plan IN ('F', 'P')"))
                .Property(u => u.Plan)
                .HasColumnType("char(1)")
                .HasDefaultValueSql("'F'");


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
        }
    }
}