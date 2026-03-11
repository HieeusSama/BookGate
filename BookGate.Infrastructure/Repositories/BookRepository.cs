using BookGate.Domain.Entities;
using BookGate.Domain.Interfaces;
using BookGate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
namespace BookGate.Infrastructure.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context){}

        new public async Task<IEnumerable<Book>> GetAll()
        => await _context.Books.Include(b => b.Publisher).ToListAsync();

        public async Task<Book?> GetById(string id)
        {
            return await _context.Books
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(b => b.BookId == id);
        }
    }
}
