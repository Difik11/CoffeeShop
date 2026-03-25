using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace CoffeeShop.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<Coffeehouse> Coffeehouses { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "Server=127.0.0.1;Port=3306;Database=coffee_shop;User=root;Password=1234;",
                new MySqlServerVersion(new Version(9, 6, 0))
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Role)
                .WithMany()
                .HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Coffeehouse)
                .WithMany()
                .HasForeignKey(e => e.CoffeeHouseId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Coffeehouse)
                .WithMany()
                .HasForeignKey(p => p.CoffeeHouseId);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductId);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Coffeehouse)
                .WithMany()
                .HasForeignKey(s => s.CoffeeHouseId);

            modelBuilder.Entity<Product>()
                .Property(p => p.Category)
                .HasDefaultValue("");

            modelBuilder.Entity<Product>()
                .Property(p => p.Unit)
                .HasDefaultValue("");
        }
    }
}
