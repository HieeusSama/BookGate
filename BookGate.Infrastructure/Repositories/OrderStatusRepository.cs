using BookGate.Infrastructure.Data;
using BookGate.Domain.Entities;
using BookGate.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGate.Infrastructure.Repositories
{
    public class OrderStatusRepository : Repository<OrderStatus>, IOrderStatusRepository
    {
        public OrderStatusRepository(ApplicationDbContext context) : base(context) { }
    }
}
