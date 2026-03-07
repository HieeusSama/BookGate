using BookGate.Domain.Interfaces;
using BookGate.Domain.Entities;
using BookGate.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BookGate.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Auth>> GetAll()
       =>  _context.Auths.ToList();

        public async Task Add(Auth auth)
        {
            _context.Auths.Add(auth);
            _context.SaveChanges();
        }

        public async Task<Auth?> Login(string email, string password)
        {
            return await _context.Auths.FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
        }
    }
}
