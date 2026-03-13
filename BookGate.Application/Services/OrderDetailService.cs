using AutoMapper;
using BookGate.Application.DTOs;
using BookGate.Domain.Entities;
using BookGate.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGate.Application.Services
{
    public class OrderDetailService
    {
        private readonly IOrderDetailRepository _repo;
        private readonly IMapper _mapper;

        public OrderDetailService(IOrderDetailRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<OrderDetailDTO>> GetAll()
        {
            var orderDetail = await _repo.GetAll();
            return _mapper.Map<List<OrderDetailDTO>>(orderDetail);
        }

        public async Task<OrderDetailDTO?> GetById(string id)
        {
            var orderDetail = await _repo.GetById(id);
            return _mapper.Map<OrderDetailDTO>(orderDetail);
        }

        public async Task<OrderDetailDTO> Add(OrderDetailDTO orderDetail)
        {
            var orderDetailEntity = _mapper.Map<OrderDetail>(orderDetail);
            await _repo.Add(orderDetailEntity);
            return _mapper.Map<OrderDetailDTO>(orderDetailEntity);
        }

        public async Task<OrderDetailDTO> Update(OrderDetailDTO orderDetail)
        {
            var orderDetailEntity = _mapper.Map<OrderDetail>(orderDetail);
            await _repo.Add(orderDetailEntity);
            return _mapper.Map<OrderDetailDTO>(orderDetailEntity);
        }

        public async Task<bool> Delete(string id)
        {
            await _repo.Delete(id);
            return true;
        }
    }
}
