using BookGate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace BookGate.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task<List<Auth>> GetAll();
        Task Add(Auth auth);
        Task<Auth?> Login(string email, string password);
    }
}
