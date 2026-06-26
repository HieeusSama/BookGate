using AutoMapper;
using BookGate.Application.DTOs;
using BookGate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGate.Application.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.OrderStatus!.StatusName))
            .ForMember(dest => dest.Fullname, opt => opt.MapFrom(src => src.Auth != null ? src.Auth.FullName : "Khách hàng ẩn danh"));

            CreateMap<OrderDTO, Order>();

            CreateMap<OrderDetail, OrderDetailDTO>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : "Sách không xác định"))
            .ForMember(dest => dest.FileUrl, opt => opt.MapFrom(src => src.Book != null ? src.Book.FileUrl : string.Empty));

            CreateMap<OrderDetailDTO, OrderDetail>();
            CreateMap<OrderStatus, OrderStatusDTO>();
            CreateMap<OrderStatusDTO, OrderStatus>();
        }
    }
}
