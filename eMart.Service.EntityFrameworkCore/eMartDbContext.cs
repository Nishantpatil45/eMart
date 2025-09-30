using eMart.Service.DataModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace eMart.Service.EntityFrameworkCore
{
    public class eMartDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<RecentlyViewed> RecentlyViewedItems { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<UserOtp> UserOtps { get; set; }


        public eMartDbContext(DbContextOptions<eMartDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(
                    "Server=localhost;Database=emart;User=root;Password=system;",
                    new MySqlServerVersion(new Version(8, 0, 34)) // Specify your MySQL version
                );
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite Keys
            modelBuilder.Entity<UserPreference>().HasKey(up => new { up.UserId, up.Category });
            modelBuilder.Entity<Favorite>().HasKey(f => new { f.UserId, f.ProductId });
            modelBuilder.Entity<RecentlyViewed>().HasKey(rv => new { rv.UserId, rv.ProductId });

            // Relationships
            modelBuilder.Entity<User>()
                .HasMany(u => u.Preferences)
                .WithOne(up => up.User)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Products)
                .WithOne(p => p.Seller)
                .HasForeignKey(p => p.SellerId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.Buyer)
                .HasForeignKey(o => o.BuyerId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.OrderItems)
                .WithOne(oi => oi.Seller)
                .HasForeignKey(oi => oi.SellerId);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Bids)
                .WithOne(b => b.Product)
                .HasForeignKey(b => b.ProductId);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<RecentlyViewed>()
                .HasOne(rv => rv.User)
                .WithMany(u => u.RecentlyViewedItems)
                .HasForeignKey(rv => rv.UserId);

            modelBuilder.Entity<UserToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.UserTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserOtp>()
                .HasOne(uo => uo.User)
                .WithMany(u => u.UserOtps)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
