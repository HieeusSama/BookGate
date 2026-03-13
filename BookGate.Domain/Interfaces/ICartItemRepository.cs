using BookGate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGate.Domain.Interfaces
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetCartItemById(int id);
    }
}
