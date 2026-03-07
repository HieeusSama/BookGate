using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using BookGate.Domain.Entities;
using BookGate.Application.DTOs;

namespace BookGate.Application.Mappings
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterDTO, Auth>().ReverseMap();
            CreateMap<Auth, LoginDTO>().ReverseMap();
        }
    }
}
