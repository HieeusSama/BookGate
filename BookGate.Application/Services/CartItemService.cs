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
            var currentCartItems = await _repo.GetCartItemById(cartItemDto.Id);
            var existingItem = currentCartItems.FirstOrDefault(c => c.BookId == cartItemDto.BookId);

            if (existingItem != null)
            {
                int quantityToAdd = cartItemDto.Quantity > 0 ? cartItemDto.Quantity : 1;

                existingItem.Quantity += quantityToAdd;
                await _repo.Update(existingItem);

                return _mapper.Map<CartItemDTO>(existingItem);
            }
            else
            {
                var cartItemEntity = _mapper.Map<CartItem>(cartItemDto);
                cartItemEntity.CartItemId = Guid.NewGuid().ToString();
                if (cartItemEntity.Quantity <= 0)
                {
                    cartItemEntity.Quantity = 1;
                }

                await _repo.Add(cartItemEntity);

                return _mapper.Map<CartItemDTO>(cartItemEntity);
            }
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

        public async Task<CartItemDTO> UpdateQuantityCartItem(string id, int quantity)
        {
            var entity = await _repo.UpdateQuantityAsync(id, quantity);

            if (entity == null)
            {
                return null;
            }

            return _mapper.Map<CartItemDTO>(entity);
        }
    }
}
