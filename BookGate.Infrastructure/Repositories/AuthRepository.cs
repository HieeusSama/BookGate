using BookGate.Domain.Interfaces;
using BookGate.Domain.Entities;
using BookGate.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BookGate.Infrastructure.Repositories
{
    public class AuthRepository : Repository<Auth>, IAuthRepository
    {
        public AuthRepository(ApplicationDbContext context) : base(context) { }


        public async Task<Auth?> Login(string email, string password)
        {
            return await _context.Auths.FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
        }
    }
}
