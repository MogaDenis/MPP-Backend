using AutoMapper;
using MPP_Backend.Business.DTOs;
using MPP_Backend.Data.Models;

namespace MPP_Backend.Business.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile() 
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserForAddUpdateDTO>().ReverseMap();
            CreateMap<UserDTO, UserForAddUpdateDTO>().ReverseMap();
        }
    }
}
