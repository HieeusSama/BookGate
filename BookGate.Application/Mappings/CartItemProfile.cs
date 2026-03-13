using AutoMapper;
using BookGate.Application.DTOs;
using BookGate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGate.Application.Mappings
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            CreateMap<CartItem, CartItemDTO>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Book!.Title))
            .ForMember(dest => dest.SellingPrice, opt => opt.MapFrom(src => src.Book!.SellingPrice))
            .ForMember(dest => dest.FileUrl, opt => opt.MapFrom(src => src.Book!.FileUrl));
            CreateMap<CartItemDTO, CartItem>();
        }
    }
}
