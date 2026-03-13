using BookGate.Domain.Entities;
using BookGate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using BookGate.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGate.Infrastructure.Repositories
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(ApplicationDbContext context) : base(context) { }
        new public async Task<IEnumerable<CartItem>> GetAll()
        => await _context.CartItems
            .Include(b => b.Auth)
            .Include(b => b.Book)
            .ToListAsync();

        public async Task<IEnumerable<CartItem>> GetCartItemById(int id)
        {
            return await _context.CartItems
            .Include(c => c.Book)
            .Where(c => c.Id == id)
            .ToListAsync();
        }
    }
}
