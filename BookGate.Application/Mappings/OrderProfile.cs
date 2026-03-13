using AutoMapper;
using BookGate.Application.DTOs;
using BookGate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using BookGate.Domain.Entities;

namespace BookGate.Application.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.OrderStatus!.StatusName));
            CreateMap<OrderDTO, Order>();
            CreateMap<OrderDetail, OrderDetailDTO>();
            CreateMap<OrderDetailDTO, OrderDetail>();
            CreateMap<OrderStatus, OrderStatusDTO>();
            CreateMap<OrderStatusDTO, OrderStatus>();
        }
    }
}
