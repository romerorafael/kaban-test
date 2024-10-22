using API.Model;
using AutoMapper;

namespace kaban_test;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<Login, LoginDTO>().ReverseMap();
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<Role, RoleDTO>().ReverseMap();
    }
}