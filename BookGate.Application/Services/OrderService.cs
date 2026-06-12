using AutoMapper;
using BookGate.Application.DTOs;
using BookGate.Domain.Entities;
using BookGate.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGate.Application.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _repo;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;

        }

        public async Task<List<OrderDTO>> GetAll()
        {
            var order = await _repo.GetAll();
            return _mapper.Map<List<OrderDTO>>(order);
        }

        public async Task<OrderDTO?> GetById(string id)
        {
            var order = await _repo.GetById(id);
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrderById(int id)
        {
            var order = await _repo.GetAllOrderById(id);
            return _mapper.Map<IEnumerable<OrderDTO>>(order);
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersWithFilter(int? userId, string searchId, string status)
        {
            var order = await _repo.GetOrdersWithFilter(userId, searchId, status);
            return _mapper.Map<IEnumerable<OrderDTO>>(order);
        }

        public async Task<OrderDTO> Add(OrderDTO order)
        {
            var orderEntity = _mapper.Map<Order>(order);
            await _repo.Add(orderEntity);
            return _mapper.Map<OrderDTO>(orderEntity);
        }

        public async Task<OrderDTO> Update(OrderDTO order)
        {
            var orderEntity = _mapper.Map<Order>(order);
            await _repo.Update(orderEntity);
            return _mapper.Map<OrderDTO>(orderEntity);
        }

        public async Task<bool> Delete(string id)
        {
            await _repo.Delete(id);
            return true;
        }
    }
}
