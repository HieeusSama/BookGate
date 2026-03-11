using System;
using System.Collections.Generic;
using System.Text;
using BookGate.Domain.Entities;

namespace BookGate.Domain.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
    }
}
