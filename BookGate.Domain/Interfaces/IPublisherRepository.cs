using BookGate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGate.Domain.Interfaces
{
    public interface IPublisherRepository
    {
        Task<List<Publisher>> GetAll();
    }
}
