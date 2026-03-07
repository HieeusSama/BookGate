using BookGate.Domain.Entities;
using BookGate.Domain.Interfaces;
using BookGate.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGate.Infrastructure.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly ApplicationDbContext _context;

        public PublisherRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Publisher>> GetAll()
       => _context.Publishers.ToList();
    }
}
