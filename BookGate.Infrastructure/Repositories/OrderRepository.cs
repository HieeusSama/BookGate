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
    }
}
