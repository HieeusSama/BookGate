using BookGate.Domain.Entities;
using BookGate.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGate.Application.Services
{
    public class PublisherService
    {
        private readonly IPublisherRepository _repo;

        public PublisherService(IPublisherRepository repo)
        {
            _repo = repo;
        }
        public Task<List<Publisher>> GetAll()
        {
            return _repo.GetAll();
        }
    }
}
