using BookGate.Domain.Entities;
using BookGate.Domain.Interfaces;
using BookGate.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGate.Infrastructure.Repositories
{
    public class PublisherRepository : Repository<Publisher>, IPublisherRepository
    {
        public PublisherRepository(ApplicationDbContext context) : base(context){}
    }
}
