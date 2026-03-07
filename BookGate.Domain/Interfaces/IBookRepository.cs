using System;
using System.Collections.Generic;
using System.Text;
using BookGate.Domain.Entities;

namespace BookGate.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAll();
        Task Add(Book book);
        Task Update(Book book);
        Task Delete(string id);
        Task<Book?> GetById(string id);
    }
}
