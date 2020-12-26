using AutoMapper;
using Domain.Model;
using dotnetWebAPI.Dtos;
using dotnetWebAPI.Dtos.UserDtos;

namespace dotnetWebAPI.Profiles
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
