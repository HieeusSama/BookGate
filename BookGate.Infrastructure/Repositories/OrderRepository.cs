using BookGate.Domain.Entities;
using BookGate.Domain.Interfaces;
using BookGate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGate.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context) { }
        new public async Task<IEnumerable<Order>> GetAll()
        => await _context.Orders
            .Include(b => b.OrderDetail)
            .Include(b => b.OrderStatus)
            .ToListAsync();
        public async Task<IEnumerable<Order>> GetOrdersWithFilter(string? orderId, string? statusId)
        {
            // Bắt đầu với truy vấn gốc
            var query = _context.Orders
                .Include(o => o.OrderStatus)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(orderId))
            {
                query = query.Where(o => o.OrderId == orderId);
            }

            if (!string.IsNullOrEmpty(statusId))
            {
                query = query.Where(o => o.StatusId == statusId);
            }

            return await query.ToListAsync();
        }
        new public async Task<Order?> GetById(object id)
        {
            return await _context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.OrderId == id);
        }
        public async Task<IEnumerable<Order?>> GetAllOrderById(int id)
        {
            return await _context.Orders
            .Include(c => c.OrderStatus)
            .Where(c => c.Id == id)
            .ToListAsync();
        }
    }
}
