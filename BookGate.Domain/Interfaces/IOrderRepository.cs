using BookGate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGate.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersWithFilter(int? userId, string searchId, string status);
        Task<IEnumerable<Order?>> GetAllOrderById(int id);
    }
}
