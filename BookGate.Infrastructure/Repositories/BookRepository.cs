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
        => await _context.Books.Include(b => b.Publisher).AsNoTracking().ToListAsync();

        public async Task<Book?> GetById(string id)
        {
            return await _context.Books
                .Include(b => b.Publisher)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BookId == id);
        }
        new public async Task Update(Book book)
        {
            _context.ChangeTracker.Clear();
            _context.Update(book);
            await _context.SaveChangesAsync();
        }
    }
}
