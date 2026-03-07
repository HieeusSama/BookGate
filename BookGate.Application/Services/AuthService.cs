using AutoMapper;
using BookGate.Application.DTOs;
using BookGate.Domain.Entities;
using BookGate.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
namespace BookGate.Application.Services
{
    public class AuthService
    {
        private readonly IAuthRepository _repo;

        private readonly IMapper _mapper;

        public AuthService(IAuthRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public Task<List<Auth>> GetAll()
        {
            return _repo.GetAll();
        }

        public async Task<Auth?> Login(LoginDTO auth) // Đổi bool thành Auth?
        {
            var user = await _repo.Login(auth.Email, auth.Password);
            return user;
        }

        public async Task<RegisterDTO> Register(RegisterDTO auth)
        {
            var authEntity = _mapper.Map<Auth>(auth);

            await _repo.Add(authEntity);
            return _mapper.Map<RegisterDTO>(authEntity);
 
        }
    }
}
