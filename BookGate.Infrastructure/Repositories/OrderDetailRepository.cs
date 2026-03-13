using BookGate.Domain.Entities;
using BookGate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using BookGate.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGate.Infrastructure.Repositories
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ApplicationDbContext context) : base(context) { }
        new public async Task<IEnumerable<OrderDetail>> GetAll()
        => await _context.OrderDetails
            .Include(b => b.Order)
            .Include(b => b.Book)
            .ToListAsync();
    }
}
