using Api.Dtos;
using Api.Dtos.UserDtos;
using AutoMapper;
using Domain.Entities;

namespace Api.Profiles
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
