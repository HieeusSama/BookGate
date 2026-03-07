using BookGate.Domain.Entities;
using BookGate.Domain.Interfaces;
using BookGate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
namespace BookGate.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {

        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAll() //
        => await _context.Books.Include(b => b.Publisher).ToListAsync();

        public async Task<Book?> GetById(string id)
        {
            return await _context.Books
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(b => b.BookId == id);
        }

        public async Task Add(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }
        public async Task Update(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(string id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
    }
}
