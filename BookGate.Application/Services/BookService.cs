using AutoMapper;
using BookGate.Application.DTOs;
using BookGate.Domain.Entities;
using BookGate.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGate.Application.Services
{
    public class BookService
    {
        private readonly IBookRepository _repo;
        private readonly IMapper _mapper;

        public BookService(IBookRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<BookDTO>> GetAll()
        {
            var books = await _repo.GetAll();
            return _mapper.Map<List<BookDTO>>(books);
        }

        public async Task<BookDTO?> GetById(string id)
        {
            var book = await _repo.GetById(id);
            return _mapper.Map<BookDTO>(book);
        }

        public async Task<BookDTO> Add(BookDTO book)
        {
            var bookEntity = _mapper.Map<Book>(book);
            bookEntity.BookId = Guid.NewGuid().ToString();
            await _repo.Add(bookEntity);
            return _mapper.Map<BookDTO>(bookEntity);
        }

        public async Task<BookDTO> Update(BookDTO book)
        {
            var bookEntity = _mapper.Map<Book>(book);
            await _repo.Update(bookEntity);
            return _mapper.Map<BookDTO>(bookEntity);
        }

        public async Task<bool> Delete(string id)
        {
            await _repo.Delete(id);
            return true;
        }
    }
}
