using AutoMapper;
using Domain.Entities;
using WebAPI.Dtos;
using WebAPI.Dtos.UserDtos;

namespace WebAPI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<User, UserReadDto>();
            CreateMap<User, UserAuditDto>().ReverseMap();
            CreateMap<UserUpdateDto, User>().ReverseMap();
            CreateMap<UserCreateDto, User>();
        }
    }
}
