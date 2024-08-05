using AutoMapper;
using Shop.Web.DTOs.Auth;
using Shop.Web.DTOs;
using Shop.Web.Models.Entity.Auth;
using Shop.Web.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.Infratructures.Helper
{
    public class MappingHelper : Profile
    {
        public MappingHelper()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<SignIn, SignInDTO>().ReverseMap();
            CreateMap<LogIn, LogInDTO>().ReverseMap();
        }
    }
}
