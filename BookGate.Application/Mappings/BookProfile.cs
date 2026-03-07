using BookGate.Application.DTOs;
using BookGate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;

namespace BookGate.Application.Mappings
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDTO>()
            .ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src => src.Publisher!.PublisherName));

            CreateMap<Book, MemberBookDTO>()
            .ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src => src.Publisher!.PublisherName));

            CreateMap<BookDTO, Book>();
            CreateMap<MemberBookDTO, Book>();
        }
    }
}