using Microsoft.EntityFrameworkCore;
using MyProject.DataAccess.Configurations;
using MyProject.Domain.Entities;
using MyProject.Infrastructure.Data.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.DataAccess
{
    public class MyDBContext : DbContext
    {
        private readonly string _connString;
        public MyDBContext(string connString)
        {
            _connString = connString;
        }

        public MyDBContext()
        {
            _connString = @"Data Source=.\SQLEXPRESS;Initial Catalog=asp_db;TrustServerCertificate=true;Integrated security = true";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfig());
            modelBuilder.ApplyConfiguration(new ProducerConfig());
            modelBuilder.ApplyConfiguration(new StatusConfig());
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new RestaurantConfig());
            modelBuilder.ApplyConfiguration(new ProductRestaurantConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.Entity<UseCaseUser>().HasKey(x => new { x.UserId, x.UseCaseUserId });
            modelBuilder.ApplyConfiguration(new PaymentConfig());
            modelBuilder.ApplyConfiguration(new OrderConfig());
            modelBuilder.ApplyConfiguration(new CartConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new OrderProductConfig());
            modelBuilder.ApplyConfiguration(new AuditLogConfig());

            modelBuilder.Entity<Status>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Category>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Producer>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Restaurant>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<ProductRestaurant>().HasQueryFilter(op => !op.Product.IsDeleted);
            modelBuilder.Entity<Cart>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Payment>().HasQueryFilter(p => p.IsActive);
            modelBuilder.Entity<Order>().HasQueryFilter(o => o.Payment.IsActive);
            modelBuilder.Entity<OrderProduct>().HasQueryFilter(op => !op.Product.IsDeleted);
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "User"},
                new Role { Id = 2, Name = "Admin" }
            );
            modelBuilder.Entity<User>().HasData(
               new User { 
                   Id = 1, 
                   Username = "Mika",
                   Email = "mika123@gmail.com",
                   Password = "mika123",
                   FirstName = "Mika",
                   LastName = "Mikic",
                   RoleId = 2,
                   IsActive = true,
                   CreatedAt = DateTime.Now
               }
              
           );

            base.OnModelCreating(modelBuilder);


        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UseCaseUser> UseCaseUsers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductRestaurant> ProductRestaurants { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
    }
}
