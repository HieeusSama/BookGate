using BookGate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace BookGate.Domain.Interfaces
{
    public interface IAuthRepository : IRepository<Auth>
    {
        Task<Auth?> Login(string email, string password);
        Task<bool> CheckEmailExistsAsync(string email);
    }
}
