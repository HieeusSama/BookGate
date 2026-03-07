using AutoMapper;
using BookGate.Application.DTOs;
using BookGate.Domain.Interfaces;
using BookGate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGate.Application.Services
{
    public class MemberBookService
    {
        private readonly IBookRepository _repo;
        private readonly IMapper _mapper;

        public MemberBookService(IBookRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<MemberBookDTO>> GetAll()
        {
            var books = await _repo.GetAll();
            return _mapper.Map<List<MemberBookDTO>>(books);
        }

        public async Task<BookDTO?> GetById(string id)
        {
            var book = await _repo.GetById(id);
            return _mapper.Map<BookDTO>(book);
        }
    }
}
