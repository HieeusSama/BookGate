using AutoMapper;
using BookGate.Application.DTOs;
using BookGate.Domain.Entities;
using BookGate.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGate.Application.Services
{
    public class CartItemService
    {
        private readonly ICartItemRepository _repo;
        private readonly IMapper _mapper;

        public CartItemService(ICartItemRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<CartItemDTO>> GetAll()
        {
            var cartIteams = await _repo.GetAll();
            return _mapper.Map<List<CartItemDTO>>(cartIteams);
        }

        public async Task<CartItemDTO?> GetById(string id)
        {
            var cartIteam = await _repo.GetById(id);
            return _mapper.Map<CartItemDTO>(cartIteam);
        }

        public async Task<CartItemDTO> Add(CartItemDTO cartItemDto)
        {
            var cartItemEntity = _mapper.Map<CartItem>(cartItemDto);
            cartItemEntity.CartItemId = Guid.NewGuid().ToString();
            await _repo.Add(cartItemEntity);
            return _mapper.Map<CartItemDTO>(cartItemEntity);
        }

        public async Task<CartItemDTO> Update(CartItemDTO cartIteam)
        {
            var cartIteamEntity = _mapper.Map<CartItem>(cartIteam);
            await _repo.Update(cartIteamEntity);
            return _mapper.Map<CartItemDTO>(cartIteamEntity);
        }

        public async Task<bool> Delete(string id)
        {
            await _repo.Delete(id);
            return true;
        }

        public async Task<List<CartItemDTO>> GetCartItemById(int id)
        {
            var cartIteams = await _repo.GetCartItemById(id);
            return _mapper.Map<List<CartItemDTO>>(cartIteams);
        }
    }
}
