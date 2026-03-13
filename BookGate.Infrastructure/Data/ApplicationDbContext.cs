using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BookGate.Domain.Entities;
namespace BookGate.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Auth> Auths { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderStatus>().HasData(
                new OrderStatus { StatusId = "PENDING", StatusName = "Chờ xử lý" },
                new OrderStatus { StatusId = "SHIPPING", StatusName = "Đang giao hàng" },
                new OrderStatus { StatusId = "COMPLETED", StatusName = "Đã hoàn thành" },
                new OrderStatus { StatusId = "CANCELLED", StatusName = "Đã hủy" }
            );
        }
    }
}
