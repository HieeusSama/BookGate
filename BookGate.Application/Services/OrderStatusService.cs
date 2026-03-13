using AutoMapper;
using BookGate.Application.DTOs;
using BookGate.Domain.Entities;
using BookGate.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGate.Application.Services
{
    public class OrderStatusService
    {
        private readonly IOrderStatusRepository _repo;
        private readonly IMapper _mapper;

        public OrderStatusService(IOrderStatusRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<OrderStatusDTO>> GetAll()
        {
            var order = await _repo.GetAll();
            return _mapper.Map<List<OrderStatusDTO>>(order);
        }

        public async Task<OrderStatusDTO?> GetById(string id)
        {
            var order = await _repo.GetById(id);
            return _mapper.Map<OrderStatusDTO>(order);
        }

        public async Task<OrderStatusDTO> Add(OrderStatusDTO order)
        {
            var orderEntity = _mapper.Map<OrderStatus>(order);
            await _repo.Add(orderEntity);
            return _mapper.Map<OrderStatusDTO>(orderEntity);
        }

        public async Task<OrderStatusDTO> Update(OrderStatusDTO cartIteam)
        {
            var orderEntity = _mapper.Map<OrderStatus>(cartIteam);
            await _repo.Update(orderEntity);
            return _mapper.Map<OrderStatusDTO>(orderEntity);
        }

        public async Task<bool> Delete(string id)
        {
            await _repo.Delete(id);
            return true;
        }
    }
}
