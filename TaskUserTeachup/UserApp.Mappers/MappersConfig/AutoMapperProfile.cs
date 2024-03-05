using AutoMapper;
using UserApp.Domain.Models;
using UserApp.DTOs.User;

namespace UserApp.Mappers.MappersConfig
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<User, LoginUserDto>().ReverseMap();
            CreateMap<User, RegisterUserDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<LoginHistory, LoginHistoryDto>().ReverseMap();
        }
    }
}
